using GroceryList.Application.Abstractions;
using GroceryList.Application.Commands.GroceryLists.UpdateGroceryList;
using GroceryList.Application.Queries.GroceryLists;
using GroceryList.Domain.Aggregates.GroceryLists;
using GroceryList.Domain.Aggregates.Stores;
using GroceryList.Domain.Exceptions;
using GroceryList.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryList.Application.Commands.Stores.UpdateStore;
public class UpdateStoreHandler : IRequestHandler<UpdateStoreCommand, Result<Store>>
{
    private readonly IStoreRepository _repository;
    private readonly IClaimReader _claimReader;

    public UpdateStoreHandler(IStoreRepository repository, IClaimReader claimReader)
    {
        repository = _repository;
        _claimReader = claimReader;
    }

    public async Task<Result<Store>> Handle(UpdateStoreCommand command, CancellationToken cancellationToken)
    {
        var store = await _repository.GetByIdAsync(command.Id);
        if (store is null)
        {
            return Result<Store>.Failure(ResultStatusCode.NotFound, $"Store with id {command.Id} was not found");
        }

        try
        {
            var userId = _claimReader.GetUserIdFromClaim();

            if (store.UserId != userId)
            {
                return Result<Store>.Failure(ResultStatusCode.ValidationError, $"Store does not belong to user {userId}");
            }

            var sections = command
              .Sections?
              .Select(x => Section.Create(x.Name, x.Priority))
              .ToList();

            var address = Address.Create(command.Street, command.City, command.Country, command.ZipCode);

            store.Update(command.Name, userId, sections, address);

            await _repository.UpdateAsync(store);

            return Result<Store>.Success(store);
        }
        catch (BusinessValidationException e)
        {
            return Result<Store>.Failure(ResultStatusCode.ValidationError, e.Errors);
        }
    }
}

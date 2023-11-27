using GroceryList.Domain.Aggregates.GroceryLists;
using GroceryList.Domain.Aggregates.Stores;
using GroceryList.Domain.Exceptions;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Commands.Stores.AddStores;

public class AddStoreHandler : IRequestHandler<AddStoreCommand, Result<Store>>
{
    private readonly IStoreRepository _repository;

    public AddStoreHandler(IStoreRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Store>> Handle(AddStoreCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var sections = command
              .Sections
              .Select(x => Section.Create(x.Name, x.Priority))
              .ToList();

            var address = Address.Create(command.Street, command.City, command.State, command.Country, command.ZipCode);
            var newStore = Store.Create(command.Name, sections, address);

            var result = await _repository.AddAsync(newStore);

            return Result<Store>.Success(result);
        }
        catch (BusinessValidationException e)
        {
            return Result<Store>.Failure(ResultStatusCode.ValidationError, e.Errors);
        }
    }
}

using GroceryList.Application.Abstractions;
using GroceryList.Application.Helpers;
using GroceryList.Application.Queries.GroceryLists;
using GroceryList.Domain.Aggregates.GroceryLists;
using GroceryList.Domain.Exceptions;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Commands.GroceryLists.UpdateGroceryList;

public class UpdateGroceryListHandler : IRequestHandler<UpdateGroceryListCommand, Result<GroceryListResponseDto>>
{
    private readonly IGroceryListRepository _repository;
    private readonly IStoreRepository _storeRepository;
    private readonly IClaimReader _claimReader;

    public UpdateGroceryListHandler(IGroceryListRepository repository, IStoreRepository storeRepository, IClaimReader claimReader)
    {
        _repository = repository;
        _storeRepository = storeRepository;
        _claimReader = claimReader;
    }

    public async Task<Result<GroceryListResponseDto>> Handle(UpdateGroceryListCommand command, CancellationToken cancellationToken)
    {
        var groceryList = await _repository.GetByIdAsync(command.Id);
        if (groceryList is null)
        {
            return Result<GroceryListResponseDto>.Failure(ResultStatusCode.NotFound, $"Grocery List with id {command.Id} was not found");
        }

        try
        {
            var ingredients = command
              .Ingredients?
              .Select(x => Ingredient.Create(x.Name, x.Amount, Category.Create(x.Category), x.Selected))
            .ToList();

            var userId = _claimReader.GetUserIdFromClaim();

            if (groceryList.UserId != userId)
            {
                return Result<GroceryListResponseDto>.Failure(ResultStatusCode.ValidationError, $"Grocery List does not belong to user {userId}");
            }

            groceryList.Update(command.Name, command.StoreId, ingredients);

            await _repository.UpdateAsync(groceryList);

            var updatedList = groceryList.ToGroceryListDto();

            if (groceryList.StoreId is not null)
            {
                updatedList.Store = await _storeRepository.GetByIdAsync((Guid)groceryList.StoreId);
            }

            return Result<GroceryListResponseDto>.Success(updatedList);
        }
        catch (BusinessValidationException e)
        {
            return Result<GroceryListResponseDto>.Failure(ResultStatusCode.ValidationError, e.Errors);
        }
    }
}

using GroceryList.Application.Abstractions;
using GroceryList.Domain.Aggregates.GroceryLists;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Commands.Stores.RemoveStore;

public class RemoveStoreHandler : IRequestHandler<RemoveStoreCommand, Result>
{
    private readonly IStoreRepository _repository;
    private readonly IGroceryListRepository _groceryListRepository;
    private readonly IClaimReader _claimReader;

    public RemoveStoreHandler(IClaimReader claimReader, IStoreRepository repository, IGroceryListRepository groceryListRepository)
    {
        _claimReader = claimReader;
        _repository = repository;
        _groceryListRepository = groceryListRepository;
    }

    public async Task<Result> Handle(RemoveStoreCommand command, CancellationToken cancellationToken)
    {
        var store = await _repository.GetByIdAsync(command.Id);
        if (store is null)
        {
            return Result.Failure(ResultStatusCode.NotFound, $"Store with id {command.Id} was not found");
        }

        var userId = _claimReader.GetUserIdFromClaim();

        if (store.UserId != userId)
        {
            return Result.Failure(ResultStatusCode.ValidationError, $"Store does not belong to user {userId}");
        }

        var lists = (await _groceryListRepository.WhereAsync(l => l.StoreId == store.Id, null, cancellationToken)).ToList();
        foreach (var list in lists)
        {
            list.UpdateIngredients(list.Ingredients.Select(i => Ingredient.Create(i.Name, i.Amount, Category.Create(string.Empty), i.Selected)).ToList());
        }
        await _groceryListRepository.UpdateRangeAsync(lists);
        await _repository.DeleteAsync(store);

        return Result.Success();
    }
}

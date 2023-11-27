using GroceryList.Domain.Aggregates.GroceryLists;
using GroceryList.Domain.Repositories;

namespace GroceryList.Infrastructure.Repositories.old;

public class GroceryListRepository
{
    private readonly List<Domain.Aggregates.GroceryLists.GroceryList> _groceryLists = new();

    public Task<Domain.Aggregates.GroceryLists.GroceryList> AddAsync(Domain.Aggregates.GroceryLists.GroceryList entity)
    {
        _groceryLists.Add(entity);
        return Task.FromResult(entity);
    }

    public Task DeleteAsync(Domain.Aggregates.GroceryLists.GroceryList entity)
    {
        _groceryLists.Remove(entity);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Domain.Aggregates.GroceryLists.GroceryList>> GetAllAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_groceryLists.AsEnumerable());
    }

    public Task<Domain.Aggregates.GroceryLists.GroceryList?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_groceryLists.FirstOrDefault(g => g.Id == id));
    }

    public Task UpdateAsync(Domain.Aggregates.GroceryLists.GroceryList entity)
    {
        var index = _groceryLists.FindIndex(item => item.Id == entity.Id);
        _groceryLists[index] = entity;
        return Task.CompletedTask;
    }
}

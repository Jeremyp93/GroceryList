using GroceryList.Domain.Aggregates.Stores;
using GroceryList.Domain.Repositories;

namespace GroceryList.Infrastructure.Repositories.old;

public class StoreRepository
{
    private readonly List<Store> _stores = new();

    public Task<Store> AddAsync(Store store)
    {
        _stores.Add(store);
        return Task.FromResult(store);
    }

    public Task DeleteAsync(Store entity)
    {
        _stores.Remove(entity);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Store>> GetAllAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_stores.AsEnumerable());
    }

    public Task<Store?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_stores.FirstOrDefault(s => s.Id == id));
    }

    public Task UpdateAsync(Store entity)
    {
        var store = _stores.Single(e => e.Id == entity.Id);
        store = entity;
        return Task.CompletedTask;
    }
}

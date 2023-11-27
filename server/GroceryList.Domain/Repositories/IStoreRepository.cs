using GroceryList.Domain.Aggregates.Stores;
using GroceryList.Domain.SeedWork;

namespace GroceryList.Domain.Repositories;

public interface IStoreRepository : IRepository<Store, Guid>
{
}

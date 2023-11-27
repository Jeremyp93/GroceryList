using GroceryList.Domain.SeedWork;

namespace GroceryList.Domain.Repositories;

public interface IGroceryListRepository : IRepository<Aggregates.GroceryLists.GroceryList, Guid>
{
}

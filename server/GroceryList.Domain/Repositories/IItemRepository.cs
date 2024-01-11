using GroceryList.Domain.Aggregates.Items;
using GroceryList.Domain.SeedWork;

namespace GroceryList.Domain.Repositories;
public interface IItemRepository : IRepository<Item, Guid>
{

}


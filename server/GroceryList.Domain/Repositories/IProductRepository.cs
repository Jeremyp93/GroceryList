using GroceryList.Domain.Aggregates.Products;
using GroceryList.Domain.SeedWork;

namespace GroceryList.Domain.Repositories;
public interface IProductRepository : IRepository<Product, Guid>
{
}

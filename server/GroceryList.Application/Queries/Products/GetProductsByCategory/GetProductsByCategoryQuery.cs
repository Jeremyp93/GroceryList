using GroceryList.Domain.Aggregates.Products;
using MediatR;

namespace GroceryList.Application.Queries.Products.GetProductsByCategory;
public record GetProductsByCategoryQuery(string CategoryId) : IRequest<Result<IEnumerable<Product>>>;

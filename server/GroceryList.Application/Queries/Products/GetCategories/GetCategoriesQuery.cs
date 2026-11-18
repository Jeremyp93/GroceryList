using GroceryList.Domain.Aggregates.Products;
using MediatR;

namespace GroceryList.Application.Queries.Products.GetCategories;
public record GetCategoriesQuery : IRequest<Result<IEnumerable<ProductCategory>>>;

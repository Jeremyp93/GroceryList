using GroceryList.Domain.Aggregates.Products;
using MediatR;

namespace GroceryList.Application.Queries.Products.GetProducts;
public record GetProductsQuery() : IRequest<Result<IEnumerable<Product>>>;

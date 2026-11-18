using GroceryList.Domain.Aggregates.Products;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Queries.Products.GetProducts;
public class GetProductsHandler : IRequestHandler<GetProductsQuery, Result<IEnumerable<Product>>>
{
    private readonly IProductRepository _repository;

    public GetProductsHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<Product>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllAsync(cancellationToken);

        return Result<IEnumerable<Product>>.Success(result);
    }
}
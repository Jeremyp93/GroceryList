using GroceryList.Domain.Aggregates.Products;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Queries.Products.GetProductsByCategory;
public class GetProductsByCategoryHandler : IRequestHandler<GetProductsByCategoryQuery, Result<IEnumerable<Product>>>
{
    private readonly IProductRepository _repository;

    public GetProductsByCategoryHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<Product>>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.WhereAsync(p => p.Category != null && p.Category.Id == request.CategoryId, cancellationToken: cancellationToken);

        return Result<IEnumerable<Product>>.Success(result);
    }
}
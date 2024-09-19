using GroceryList.Domain.Aggregates.Products;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Queries.Products.GetCategories;
public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, Result<IEnumerable<ProductCategory>>>
{
    private readonly IProductRepository _repository;

    public GetCategoriesHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<ProductCategory>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.WhereAsync(x => x.Category != null, cancellationToken: cancellationToken);
        var categories = products.Select(p => p.Category).DistinctBy(x => x!.Id) as IEnumerable<ProductCategory>;

        return Result<IEnumerable<ProductCategory>>.Success(categories);
    }
}

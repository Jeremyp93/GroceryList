using GroceryList.Domain.Aggregates.Stores;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Queries.Stores.GetStores;

public class GetStoresHandler : IRequestHandler<GetStoresQuery, Result<IEnumerable<Store>>>
{
    private readonly IStoreRepository _repository;

    public GetStoresHandler(IStoreRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<Store>>> Handle(GetStoresQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllAsync(cancellationToken);

        return Result<IEnumerable<Store>>.Success(result);
    }

}

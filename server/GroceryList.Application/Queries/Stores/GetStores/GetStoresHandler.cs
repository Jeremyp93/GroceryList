using GroceryList.Application.Abstractions;
using GroceryList.Domain.Aggregates.Stores;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Queries.Stores.GetStores;

public class GetStoresHandler : IRequestHandler<GetStoresQuery, Result<IEnumerable<Store>>>
{
    private readonly IStoreRepository _repository;
    private readonly IClaimReader _claimReader;

    public GetStoresHandler(IStoreRepository repository, IClaimReader claimReader)
    {
        _repository = repository;
        _claimReader = claimReader;
    }

    public async Task<Result<IEnumerable<Store>>> Handle(GetStoresQuery request, CancellationToken cancellationToken)
    {
        var userId = _claimReader.GetUserIdFromClaim();
        var result = await _repository.WhereAsync(l => l.UserId == userId);

        return Result<IEnumerable<Store>>.Success(result);
    }

}

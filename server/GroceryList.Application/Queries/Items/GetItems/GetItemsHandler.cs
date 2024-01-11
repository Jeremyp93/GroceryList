using GroceryList.Application.Abstractions;
using GroceryList.Domain.Aggregates.Items;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Queries.Items.GetItems;
public class GetItemsHandler : IRequestHandler<GetItemsQuery, Result<IEnumerable<Item>>>
{
    private readonly IItemRepository _repository;
    private readonly IClaimReader _claimReader;

    public GetItemsHandler(IItemRepository repository, IClaimReader claimReader)
    {
        _repository = repository;
        _claimReader = claimReader;
    }

    public async Task<Result<IEnumerable<Item>>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
    {
        var userId = _claimReader.GetUserIdFromClaim();
        var result = await _repository.WhereAsync(l => l.UserId == userId, null, cancellationToken);

        return Result<IEnumerable<Item>>.Success(result);
    }
}

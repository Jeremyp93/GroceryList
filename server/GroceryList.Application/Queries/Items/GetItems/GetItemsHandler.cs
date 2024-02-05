using GroceryList.Domain.Aggregates.Items;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Queries.Items.GetItems;
public class GetItemsHandler : IRequestHandler<GetItemsQuery, Result<IEnumerable<Item>>>
{
    private readonly IItemRepository _repository;

    public GetItemsHandler(IItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<Item>>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllAsync(cancellationToken: cancellationToken);

        return Result<IEnumerable<Item>>.Success(result);
    }
}

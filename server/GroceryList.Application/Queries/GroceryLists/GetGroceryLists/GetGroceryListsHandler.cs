using GroceryList.Application.Helpers;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Queries.GroceryLists.GetGroceryLists;

public class GetGroceryListsHandler : IRequestHandler<GetGroceryListsQuery, Result<IEnumerable<GroceryListResponseDto>>>
{
    private readonly IGroceryListRepository _repository;
    private readonly IStoreRepository _storeRepository;

    public GetGroceryListsHandler(IGroceryListRepository repository, IStoreRepository storeRepository)
    {
        _repository = repository;
        _storeRepository = storeRepository;
    }

    public async Task<Result<IEnumerable<GroceryListResponseDto>>> Handle(GetGroceryListsQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetAllAsync(cancellationToken);
        var stores = await _storeRepository.GetAllAsync(cancellationToken);
        var lists = new List<GroceryListResponseDto>();
        foreach (var list in result)
        {
            var newList = list.ToGroceryListDto();
            
            if (list.StoreId.HasValue)
            {
                newList.Store = stores.SingleOrDefault(s => s.Id == list.StoreId);
            }
            lists.Add(newList);
        }


        return Result<IEnumerable<GroceryListResponseDto>>.Success(lists);
    }

}

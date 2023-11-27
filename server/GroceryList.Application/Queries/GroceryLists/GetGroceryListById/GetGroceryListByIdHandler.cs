using GroceryList.Application.Helpers;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Queries.GroceryLists.GetGroceryListById;

public class GetGroceryListByIdHandler : IRequestHandler<GetGroceryListByIdQuery, Result<GroceryListResponseDto>>
{
    private readonly IGroceryListRepository _repository;
    private readonly IStoreRepository _storeRepository;

    public GetGroceryListByIdHandler(IGroceryListRepository repository, IStoreRepository storeRepository)
    {
        _repository = repository;
        _storeRepository = storeRepository;
    }

    public async Task<Result<GroceryListResponseDto>> Handle(GetGroceryListByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(request.id);

        if (result is null)
        {
            return Result<GroceryListResponseDto>.Failure(ResultStatusCode.NotFound, $"Grocery List with id {request.id} was not found");
        }

        var newList = result.ToGroceryListDto();
        if (result.StoreId.HasValue)
        {
            newList.Store = await _storeRepository.GetByIdAsync((Guid)result.StoreId);
        }

        return Result<GroceryListResponseDto>.Success(newList);
    }
}

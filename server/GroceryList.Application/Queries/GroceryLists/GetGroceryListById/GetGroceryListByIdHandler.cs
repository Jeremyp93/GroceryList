using GroceryList.Application.Abstractions;
using GroceryList.Application.Helpers;
using GroceryList.Domain.Aggregates.GroceryLists;
using GroceryList.Domain.Repositories;
using MediatR;
using System.Collections.Generic;

namespace GroceryList.Application.Queries.GroceryLists.GetGroceryListById;

public class GetGroceryListByIdHandler : IRequestHandler<GetGroceryListByIdQuery, Result<GroceryListResponseDto>>
{
    private readonly IGroceryListRepository _repository;
    private readonly IStoreRepository _storeRepository;
    private readonly IClaimReader _claimReader;

    public GetGroceryListByIdHandler(IGroceryListRepository repository, IStoreRepository storeRepository, IClaimReader claimReader)
    {
        _repository = repository;
        _storeRepository = storeRepository;
        _claimReader = claimReader;
    }

    public async Task<Result<GroceryListResponseDto>> Handle(GetGroceryListByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(request.id);

        if (result is null)
        {
            return Result<GroceryListResponseDto>.Failure(ResultStatusCode.NotFound, $"Grocery List with id {request.id} was not found");
        }

        var userId = _claimReader.GetUserIdFromClaim();

        if (result.UserId != userId)
        {
            return Result<GroceryListResponseDto>.Failure(ResultStatusCode.ValidationError, $"Grocery List does not belong to user {userId}");
        }

        var newList = result.ToGroceryListDto();
        if (result.StoreId.HasValue)
        {
            newList.Store = await _storeRepository.GetByIdAsync((Guid)result.StoreId);
        }

        return Result<GroceryListResponseDto>.Success(newList);
    }
}

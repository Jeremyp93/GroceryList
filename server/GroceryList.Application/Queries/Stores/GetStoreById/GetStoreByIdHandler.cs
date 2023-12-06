using GroceryList.Application.Abstractions;
using GroceryList.Application.Queries.GroceryLists;
using GroceryList.Domain.Aggregates.Stores;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Queries.Stores.GetStoreById;

public class GetStoreByIdHandler : IRequestHandler<GetStoreByIdQuery, Result<Store>>
{
    private readonly IStoreRepository _repository;
    private readonly IClaimReader _claimReader;

    public GetStoreByIdHandler(IStoreRepository repository, IClaimReader claimReader)
    {
        _repository = repository;
        _claimReader = claimReader;
    }

    public async Task<Result<Store>> Handle(GetStoreByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(request.id);

        if (result is null)
        {
            Result<Store>.Failure(ResultStatusCode.NotFound, $"Store with id {request.id} was not found");
        }

        var userId = _claimReader.GetUserIdFromClaim();

        if (result.UserId != userId)
        {
            return Result<Store>.Failure(ResultStatusCode.ValidationError, $"Store does not belong to user {userId}");
        }

        return Result<Store>.Success(result);
    }
}

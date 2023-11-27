using GroceryList.Domain.Aggregates.Stores;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Queries.Stores.GetStoreById;

public class GetStoreByIdHandler : IRequestHandler<GetStoreByIdQuery, Result<Store>>
{
    private readonly IStoreRepository _repository;

    public GetStoreByIdHandler(IStoreRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Store>> Handle(GetStoreByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(request.id);

        return result is null
            ? Result<Store>.Failure(ResultStatusCode.NotFound, $"Store with id {request.id} was not found")
            : Result<Store>.Success(result);
    }
}

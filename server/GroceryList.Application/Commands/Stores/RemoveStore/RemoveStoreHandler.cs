using GroceryList.Application.Abstractions;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Commands.Stores.RemoveStore;

public class RemoveStoreHandler : IRequestHandler<RemoveStoreCommand, Result>
{
    private readonly IStoreRepository _repository;
    private readonly IClaimReader _claimReader;

    public RemoveStoreHandler(IClaimReader claimReader, IStoreRepository repository)
    {
        _claimReader = claimReader;
        _repository = repository;
    }

    public async Task<Result> Handle(RemoveStoreCommand command, CancellationToken cancellationToken)
    {
        var store = await _repository.GetByIdAsync(command.Id);
        if (store is null)
        {
            return Result.Failure(ResultStatusCode.NotFound, $"Store with id {command.Id} was not found");
        }

        var userId = _claimReader.GetUserIdFromClaim();

        if (store.UserId != userId)
        {
            return Result.Failure(ResultStatusCode.ValidationError, $"Store does not belong to user {userId}");
        }

        await _repository.DeleteAsync(store);

        return Result.Success();
    }
}

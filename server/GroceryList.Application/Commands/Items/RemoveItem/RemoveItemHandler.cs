using GroceryList.Application.Abstractions;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Commands.Items.RemoveItem;
internal class RemoveItemHandler : IRequestHandler<RemoveItemCommand, Result>
{
    private readonly IItemRepository _repository;
    private readonly IClaimReader _claimReader;

    public RemoveItemHandler(IClaimReader claimReader, IItemRepository repository)
    {
        _claimReader = claimReader;
        _repository = repository;
    }

    public async Task<Result> Handle(RemoveItemCommand command, CancellationToken cancellationToken)
    {
        var item = await _repository.GetByIdAsync(command.Id);
        if (item is null)
        {
            return Result.Failure(ResultStatusCode.NotFound, $"Item with id {command.Id} was not found");
        }

        var userId = _claimReader.GetUserIdFromClaim();

        if (item.UserId != userId)
        {
            return Result.Failure(ResultStatusCode.ValidationError, $"Item does not belong to user {userId}");
        }

        await _repository.DeleteAsync(item);

        return Result.Success();
    }
}

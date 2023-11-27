using GroceryList.Application.Abstractions;
using GroceryList.Application.Queries.GroceryLists;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Commands.GroceryLists.RemoveGroceryList;
public class RemoveGroceryListHandler : IRequestHandler<RemoveGroceryListCommand, Result<string>>
{
    private readonly IGroceryListRepository _groceryListRepository;
    private readonly IClaimReader _claimReader;

    public RemoveGroceryListHandler(IGroceryListRepository groceryListRepository, IClaimReader claimReader)
    {
        _groceryListRepository = groceryListRepository;
        _claimReader = claimReader;
    }

    public async Task<Result<string>> Handle(RemoveGroceryListCommand command, CancellationToken cancellationToken)
    {
        var groceryList = await _groceryListRepository.GetByIdAsync(command.Id);
        if (groceryList is null)
        {
            return Result<string>.Failure(ResultStatusCode.NotFound, $"Grocery List with id {command.Id} was not found");
        }

        var userId = _claimReader.GetUserIdFromClaim();

        if (groceryList.UserId != userId)
        {
            return Result<string>.Failure(ResultStatusCode.ValidationError, $"Grocery List does not belong to user {userId}");
        }

        await _groceryListRepository.DeleteAsync(groceryList);

        return Result<string>.Success("");
    }
}

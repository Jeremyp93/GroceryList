using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Commands.GroceryLists.RemoveGroceryList;
public class RemoveGroceryListHandler : IRequestHandler<RemoveGroceryListCommand, Result<string>>
{
    private readonly IGroceryListRepository _groceryListRepository;

    public RemoveGroceryListHandler(IGroceryListRepository groceryListRepository)
    {
        _groceryListRepository = groceryListRepository;
    }

    public async Task<Result<string>> Handle(RemoveGroceryListCommand command, CancellationToken cancellationToken)
    {
        var groceryList = await _groceryListRepository.GetByIdAsync(command.Id);
        if (groceryList is null)
        {
            return Result<string>.Failure(ResultStatusCode.NotFound, $"Grocery List with id {command.Id} was not found");
        }

        await _groceryListRepository.DeleteAsync(groceryList);

        return Result<string>.Success("");
    }
}

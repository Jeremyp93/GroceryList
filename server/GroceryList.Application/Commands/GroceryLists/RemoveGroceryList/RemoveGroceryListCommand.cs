using MediatR;

namespace GroceryList.Application.Commands.GroceryLists.RemoveGroceryList;

public record RemoveGroceryListCommand(Guid Id) : IRequest<Result<string>>;

using MediatR;

namespace GroceryList.Application.Commands.Items.RemoveItem;
public record RemoveItemCommand(Guid Id) : IRequest<Result>;

using GroceryList.Domain.Aggregates.Items;
using MediatR;

namespace GroceryList.Application.Commands.Items.AddItem;
public record AddItemCommand(string Name) : IRequest<Result<Item>>
{
}

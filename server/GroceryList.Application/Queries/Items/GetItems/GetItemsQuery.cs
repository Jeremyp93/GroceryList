using GroceryList.Domain.Aggregates.Items;
using MediatR;

namespace GroceryList.Application.Queries.Items.GetItems;
public record GetItemsQuery() : IRequest<Result<IEnumerable<Item>>>;

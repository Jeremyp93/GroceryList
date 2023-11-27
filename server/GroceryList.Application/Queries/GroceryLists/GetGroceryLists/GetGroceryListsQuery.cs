using MediatR;

namespace GroceryList.Application.Queries.GroceryLists.GetGroceryLists;

public record GetGroceryListsQuery() : IRequest<Result<IEnumerable<GroceryListResponseDto>>>;

using MediatR;

namespace GroceryList.Application.Queries.GroceryLists.GetGroceryListById;

public record GetGroceryListByIdQuery(Guid id) : IRequest<Result<GroceryListResponseDto>>;

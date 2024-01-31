using GroceryList.Domain.Aggregates.Users;
using MediatR;

namespace GroceryList.Application.Queries.Users.GetUserFromTwitch;
public record GetUserFromTwitchQuery(string Code) : IRequest<Result<User>>
{
}

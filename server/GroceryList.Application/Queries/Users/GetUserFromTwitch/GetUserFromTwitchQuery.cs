using GroceryList.Application.Models;
using MediatR;

namespace GroceryList.Application.Queries.Users.GetUserFromTwitch;
public record GetUserFromTwitchQuery(string Code) : IRequest<Result<ApplicationUser>>;

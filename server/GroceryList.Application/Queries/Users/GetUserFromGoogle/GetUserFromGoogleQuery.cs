using GroceryList.Application.Models;
using MediatR;

namespace GroceryList.Application.Queries.Users.GetUserFromGoogle;
public record GetUserFromGoogleQuery(string Code) : IRequest<Result<ApplicationUser>>;

using GroceryList.Application.Models;
using MediatR;

namespace GroceryList.Application.Queries.Users.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<Result<ApplicationUser>>;

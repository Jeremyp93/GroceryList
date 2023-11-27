using GroceryList.Domain.Aggregates.Users;
using MediatR;

namespace GroceryList.Application.Queries.Users.GetUserById;

public record GetUserByIdQuery(Guid id) : IRequest<Result<User>>;

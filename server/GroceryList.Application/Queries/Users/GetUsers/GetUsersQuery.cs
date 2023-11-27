using GroceryList.Domain.Aggregates.Users;
using MediatR;

namespace GroceryList.Application.Queries.Users.GetUsers;

public record GetUsersQuery() : IRequest<Result<IEnumerable<User>>>;

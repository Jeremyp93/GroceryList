using GroceryList.Domain.Aggregates.Users;
using MediatR;

namespace GroceryList.Application.Commands.Users.AddUser;

public record AddUserCommand() : IRequest<Result<User>>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

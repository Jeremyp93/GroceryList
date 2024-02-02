using GroceryList.Application.Models;
using MediatR;

namespace GroceryList.Application.Commands.Users.AddUser;

public record AddUserCommand() : IRequest<Result<ApplicationUser>>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

using GroceryList.Application.Models;
using GroceryList.Domain.Aggregates.Users;
using MediatR;

namespace GroceryList.Application.Commands.Login;
public record LoginCommand() : IRequest<Result<ApplicationUser>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
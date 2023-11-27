using MediatR;

namespace GroceryList.Application.Commands.Login;
public record LoginCommand() : IRequest<Result<string>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
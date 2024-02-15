using MediatR;

namespace GroceryList.Application.Commands.Users.ResetPassword;
public record ResetPasswordCommand : IRequest<Result>
{
    public required string Email { get; set; }
    public required string Token { get; set; }
    public required string Password { get; set; }
}

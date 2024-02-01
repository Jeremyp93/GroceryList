using GroceryList.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GroceryList.Application.Commands.Login;
public class LoginHandler : IRequestHandler<LoginCommand, Result<ApplicationUser>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<ApplicationUser>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            return Result<ApplicationUser>.Failure(ResultStatusCode.ValidationError, "Invalid credentials.");
        }

        if (user.PasswordHash is null)
        {
            return Result<ApplicationUser>.Failure(ResultStatusCode.ValidationError, "Invalid credentials.");
        }

        if (!await _userManager.CheckPasswordAsync(user, command.Password))
        {
            return Result<ApplicationUser>.Failure(ResultStatusCode.ValidationError, "Invalid credentials.");
        }

        if (!user.EmailConfirmed)
        {
            return Result<ApplicationUser>.Failure(ResultStatusCode.ValidationError, "Email not confirmed.");
        }

        return Result<ApplicationUser>.Success(user);
    }
}

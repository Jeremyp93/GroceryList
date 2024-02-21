using GroceryList.Application.Helpers;
using GroceryList.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GroceryList.Application.Commands.Users.ResetPassword;
public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ResetPasswordHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            return Result.Failure(ResultStatusCode.NotFound, "Email does not exist.");
        }
        var resetToken = Base64Helper.DecodeFrom64(command.Token);
        var result  = await _userManager.ResetPasswordAsync(user, resetToken, command.Password);
        return result.Succeeded ? Result.Success() : Result.Failure(ResultStatusCode.Error, "Password could not be reset.");
    }
}

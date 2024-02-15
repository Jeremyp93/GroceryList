using GroceryList.Application.Abstractions;
using GroceryList.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GroceryList.Application.Commands.Users.ForgotPassword;
public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    public ForgotPasswordHandler(UserManager<ApplicationUser> userManager, IEmailService emailService)
    {
        _userManager = userManager;
        _emailService = emailService;
    }
    public async Task<Result> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            return Result.Failure(ResultStatusCode.ValidationError, "Email does not exist.");
        }
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        await _emailService.SendForgotPasswordLink(user.Email, token);
        return Result.Success();
    }
}

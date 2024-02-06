using GroceryList.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace GroceryList.Application.Commands.Users.ConfirmEmail;
public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, Result>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ConfirmEmailHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(ConfirmEmailCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(command.Email.ToLower());
        if (user == null)
            return Result.Failure(ResultStatusCode.NotFound, "User was not found.");
        var token = DecodeFrom64(command.Token);
        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded ? Result.Success() : Result.Failure(ResultStatusCode.Error, "Email could not be confirmed.");
    }
    private static string DecodeFrom64(string encodedData)
    {
        byte[] encodedDataAsBytes
            = Convert.FromBase64String(encodedData);

        string returnValue =
           ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);

        return returnValue;
    }
}

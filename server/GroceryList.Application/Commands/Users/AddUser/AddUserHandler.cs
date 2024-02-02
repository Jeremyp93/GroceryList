using GroceryList.Application.Abstractions;
using GroceryList.Application.Models;
using GroceryList.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace GroceryList.Application.Commands.Users.AddUser;

public class AddUserHandler : IRequestHandler<AddUserCommand, Result<ApplicationUser>>
{
    private readonly IEmailService _emailService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AddUserHandler(IEmailService emailService, UserManager<ApplicationUser> userManager)
    {
        _emailService = emailService;
        _userManager = userManager;
    }

    public async Task<Result<ApplicationUser>> Handle(AddUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(command.Email.ToLower());
        if (user is not null && !string.IsNullOrEmpty(user.PasswordHash))
        {
            return Result<ApplicationUser>.Failure(ResultStatusCode.ValidationError, "Email is already taken");
        }

        ApplicationUser newUser;
        if (user is not null)
        {
            user.FirstName = command.FirstName;
            user.LastName = command.LastName;
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, command.Password);
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return Result<ApplicationUser>.Failure(ResultStatusCode.Error, "User could not be created.");
            }
            newUser = user;
        }
        else
        {
            newUser = new ApplicationUser
            {
                Email = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,
                UserName = command.Email
            };
            var createResult = await _userManager.CreateAsync(newUser, command.Password);
            if (!createResult.Succeeded)
            {
                return Result<ApplicationUser>.Failure(ResultStatusCode.Error, "User could not be created.");
            }
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
        await _emailService.SendTokenEmailAsync(newUser.Email, token);

        return Result<ApplicationUser>.Success(newUser);
    }
}

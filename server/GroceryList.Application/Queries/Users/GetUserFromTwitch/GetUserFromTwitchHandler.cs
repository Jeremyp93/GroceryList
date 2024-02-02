using GroceryList.Application.Abstractions;
using GroceryList.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace GroceryList.Application.Queries.Users.GetUserFromTwitch;
public class GetUserFromTwitchHandler : IRequestHandler<GetUserFromTwitchQuery, Result<ApplicationUser>>
{
    private const string TwitchKey = "twitch";
    private readonly ITwitchClient _twitchClient;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserFromTwitchHandler(ITwitchClient twitchClient, UserManager<ApplicationUser> userManager)
    {
        _twitchClient = twitchClient;
        _userManager = userManager;
    }

    public async Task<Result<ApplicationUser>> Handle(GetUserFromTwitchQuery request, CancellationToken cancellationToken)
    {
        var twitchUser = await _twitchClient.GetUser(request.Code);
        if (twitchUser is null)
        {
            return Result<ApplicationUser>.Failure(ResultStatusCode.ValidationError, "User info could not be retrieved.");
        }

        var existingUser = await _userManager.FindByEmailAsync(twitchUser.Email.ToLower());
        if (existingUser is not null && existingUser.OAuthProviders.Any(o => o.OAuthProviderId == twitchUser.Id && o.OAuthProviderName == TwitchKey))
        {
            return Result<ApplicationUser>.Success(existingUser);
        }

        var provider = OAuthProvider.Create(twitchUser.Id, TwitchKey);
        if (existingUser is not null)
        {
            existingUser.OAuthProviders.Add(provider);
            var updateResult = await _userManager.UpdateAsync(existingUser);
            return updateResult.Succeeded ? Result<ApplicationUser>.Success(existingUser) : Result<ApplicationUser>.Failure(ResultStatusCode.Error, "User could not been logged in.");
        }

        var newUser = new ApplicationUser
        {
            UserName = twitchUser.Email,
            Email = twitchUser.Email,
            FirstName = twitchUser.DisplayName,
        };
        newUser.OAuthProviders.Add(provider);

        var createResult = await _userManager.CreateAsync(newUser);

        return createResult.Succeeded ? Result<ApplicationUser>.Success(newUser) : Result<ApplicationUser>.Failure(ResultStatusCode.Error, "User could not been logged in.");
    }
}

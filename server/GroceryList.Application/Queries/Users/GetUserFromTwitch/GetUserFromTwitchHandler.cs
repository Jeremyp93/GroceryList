using GroceryList.Application.Abstractions;
using GroceryList.Domain.Aggregates.Users;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Queries.Users.GetUserFromTwitch;
public class GetUserFromTwitchHandler : IRequestHandler<GetUserFromTwitchQuery, Result<User>>
{
    private readonly ITwitchClient _twitchClient;
    private readonly IUserRepository _repository;

    public GetUserFromTwitchHandler(ITwitchClient twitchClient, IUserRepository repository)
    {
        _twitchClient = twitchClient;
        _repository = repository;
    }

    public async Task<Result<User>> Handle(GetUserFromTwitchQuery request, CancellationToken cancellationToken)
    {
        var twitchUser = await _twitchClient.GetUser(request.Code);
        if (twitchUser is null)
        {
            return Result<User>.Failure(ResultStatusCode.ValidationError, "User info could not be retrieved.");
        }

        var existingUser = await _repository.SingleOrDefaultAsync(u => u.OAuthProviders.Any(o => o.OAuthProviderId == twitchUser.Id && o.OAuthProviderName == "twitch"));
        if (existingUser is not null) 
        {
            return Result<User>.Success(existingUser);
        }

        existingUser = await _repository.SingleOrDefaultAsync(u => u.Email.ToLowerInvariant() == twitchUser.Email.ToLowerInvariant());
        if (existingUser is not null)
        {
            var updatedUser = User.Create(existingUser.Id, existingUser.FirstName, existingUser.LastName, existingUser.Email, existingUser.Password, existingUser.OAuthProviders.ToList());
            updatedUser.AddOAuthProvider(OAuthProvider.Create(twitchUser.Id, "twitch"));
            await _repository.UpdateAsync(updatedUser);
            return Result<User>.Success(updatedUser);
        }
        else
        {
            var newUser = User.Create(Guid.NewGuid(), twitchUser.DisplayName, null, twitchUser.Email, null);
            newUser.AddOAuthProvider(OAuthProvider.Create(twitchUser.Id, "twitch"));

            var createdUser = await _repository.AddAsync(newUser);

            return Result<User>.Success(createdUser);
        }
    }
}

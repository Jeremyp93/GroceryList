using GroceryList.Application.Abstractions;
using GroceryList.Domain.Aggregates.Users;
using GroceryList.Domain.Repositories;
using MediatR;

namespace GroceryList.Application.Queries.Users.GetUserFromTwitch;
public class GetUserFromTwitchHandler : IRequestHandler<GetUserFromTwitchQuery, Result<User>>
{
    private const string TwitchKey = "twitch";
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

        var existingUser = await _repository.SingleOrDefaultAsync(u => u.OAuthProviders.Any(o => o.OAuthProviderId == twitchUser.Id && o.OAuthProviderName == TwitchKey));
        if (existingUser is not null) 
        {
            return Result<User>.Success(existingUser);
        }

        existingUser = await _repository.SingleOrDefaultAsync(u => u.Email.ToLowerInvariant() == twitchUser.Email.ToLowerInvariant());
        if (existingUser is not null)
        {
            existingUser.AddOAuthProvider(OAuthProvider.Create(twitchUser.Id, TwitchKey));
            await _repository.UpdateAsync(existingUser);
            return Result<User>.Success(existingUser);
        }
        else
        {
            var newUser = User.Create(Guid.NewGuid(), twitchUser.DisplayName, null, twitchUser.Email, null);
            newUser.AddOAuthProvider(OAuthProvider.Create(twitchUser.Id, TwitchKey));

            var createdUser = await _repository.AddAsync(newUser);

            return Result<User>.Success(createdUser);
        }
    }
}

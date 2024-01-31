using MediatR;
using Microsoft.Extensions.Options;
using System.Web;

namespace GroceryList.Application.Commands.LoginTwitch;
public class AuthorizeTwitchHandler : IRequestHandler<AuthorizeTwitchCommand, Result<string>>
{
    private readonly TwitchOptions _twitchConfiguration;

    public AuthorizeTwitchHandler(IOptions<TwitchOptions> options)
    {
        _twitchConfiguration = options.Value;
    }

    public Task<Result<string>> Handle(AuthorizeTwitchCommand command, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result<string>.Success($"{_twitchConfiguration.AuthorizationEndpoint}?response_type=code&client_id={_twitchConfiguration.ClientId}&redirect_uri={_twitchConfiguration.CallbackUrl}&scope={HttpUtility.UrlEncode($"{_twitchConfiguration.Scope}")}"));
    }
}

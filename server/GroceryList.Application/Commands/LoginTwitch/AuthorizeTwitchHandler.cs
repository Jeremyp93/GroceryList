using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Web;

namespace GroceryList.Application.Commands.LoginTwitch;
public class AuthorizeTwitchHandler : IRequestHandler<AuthorizeTwitchCommand, Result<string>>
{
    private readonly TwitchOptions _twitchConfiguration;
    private readonly ILogger<AuthorizeTwitchHandler> _logger;

    public AuthorizeTwitchHandler(IOptions<TwitchOptions> options, ILogger<AuthorizeTwitchHandler> logger)
    {
        _twitchConfiguration = options.Value;
        _logger = logger;
    }

    public Task<Result<string>> Handle(AuthorizeTwitchCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Login twitch options: {JsonSerializer.Serialize(_twitchConfiguration)}");
        return Task.FromResult(Result<string>.Success($"{_twitchConfiguration.AuthorizationEndpoint}?response_type=code&client_id={_twitchConfiguration.ClientId}&redirect_uri={_twitchConfiguration.CallbackUrl}&scope={HttpUtility.UrlEncode($"{_twitchConfiguration.Scope}")}"));
    }
}

using MediatR;
using Microsoft.Extensions.Options;
using System.Web;

namespace GroceryList.Application.Commands.LoginGoogle;
public class AuthorizeGmailHandler : IRequestHandler<AuthorizeGoogleCommand, Result<string>>
{
    private readonly GoogleOptions _googleConfiguration;

    public AuthorizeGmailHandler(IOptions<GoogleOptions> options)
    {
        _googleConfiguration = options.Value;
    }

    public Task<Result<string>> Handle(AuthorizeGoogleCommand command, CancellationToken cancellationToken)
    {
        return Task.FromResult(Result<string>.Success($"{_googleConfiguration.AuthorizationEndpoint}?response_type=code&client_id={_googleConfiguration.ClientId}&redirect_uri={_googleConfiguration.CallbackUrl}&scope={HttpUtility.UrlEncode($"{_googleConfiguration.Scope}")}"));
    }
}

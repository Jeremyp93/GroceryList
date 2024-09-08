using GroceryList.Application.Abstractions;
using GroceryList.Application.Commands.LoginTwitch;
using GroceryList.Application.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GroceryList.Infrastructure.Services;

public class TwitchClient : OAuthClient<TwitchOptions, TwitchUser, TwitchTokenResponse>, ITwitchClient
{
    public TwitchClient(HttpClient twitchHttpClient, IHttpClientFactory httpClientFactory, IOptions<TwitchOptions> options)
        : base(twitchHttpClient, httpClientFactory, options)
    { }

    protected override string GetUserInfoEndpoint()
    {
        return "users";
    }

    protected override string? ExtractAccessToken(TwitchTokenResponse? tokenResponse)
    {
        return tokenResponse?.AccessToken;
    }

    protected override TwitchUser? GetUserInfoFromResponse(string responseString)
    {
        var usersResponse = JsonSerializer.Deserialize<TwitchUsersResponse>(responseString);
        return usersResponse?.Data.FirstOrDefault();
    }
}

public class TwitchTokenResponse
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; set; }
    [JsonPropertyName("expires_in")]
    public required int ExpiresIn { get; set; }
    [JsonPropertyName("refresh_token")]
    public required string RefreshTtoken { get; set; }
    [JsonPropertyName("scope")]
    public required string[] Scope { get; set; }
    [JsonPropertyName("token_type")]
    public required string TokenType { get; set; }
}


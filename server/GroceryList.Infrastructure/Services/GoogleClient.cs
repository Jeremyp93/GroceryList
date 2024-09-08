using GroceryList.Application.Abstractions;
using GroceryList.Application.Commands.LoginGoogle;
using GroceryList.Application.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GroceryList.Infrastructure.Services;

public class GoogleClient : OAuthClient<GoogleOptions, GoogleUser, GoogleTokenResponse>, IGoogleClient
{
    public GoogleClient(HttpClient googleHttpClient, IHttpClientFactory httpClientFactory, IOptions<GoogleOptions> options)
        : base(googleHttpClient, httpClientFactory, options)
    { }

    protected override string GetUserInfoEndpoint()
    {
        return "userinfo?alt=json";
    }

    protected override string? ExtractAccessToken(GoogleTokenResponse? tokenResponse)
    {
        if (tokenResponse?.IsSuccess ?? false)
        {
            return tokenResponse.AccessToken;
        }

        throw new Exception(tokenResponse?.error_description ?? "Unknown error during authentication");
    }

    protected override GoogleUser? GetUserInfoFromResponse(string responseString)
    {
        return JsonSerializer.Deserialize<GoogleUser>(responseString);
    }
}

public class GoogleTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    [JsonPropertyName("scope")]
    public string Scope { get; set; } = string.Empty;
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = string.Empty;
    [JsonPropertyName("id_token")]
    public string IdToken { get; set; } = string.Empty;
    public string error { get; set; } = string.Empty;
    public string error_description { get; set; } = string.Empty;
    public bool IsSuccess => string.IsNullOrEmpty(error);
}

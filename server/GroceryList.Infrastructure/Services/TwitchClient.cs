using GroceryList.Application.Abstractions;
using GroceryList.Application.Commands.LoginTwitch;
using GroceryList.Application.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GroceryList.Infrastructure.Services;
public class TwitchClient : ITwitchClient
{
    private readonly HttpClient _twitchHttpClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly TwitchOptions _options;

    public TwitchClient(HttpClient twitchHttpClient, IHttpClientFactory httpClientFactory, IOptions<TwitchOptions> options)
    {
        _twitchHttpClient = twitchHttpClient ?? throw new ArgumentNullException(nameof(twitchHttpClient));
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
    }

    public async Task<TwitchUser?> GetUser(string code)
    {
        var token = await Authenticate(code);

        // Set headers
        _twitchHttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        _twitchHttpClient.DefaultRequestHeaders.Add("Client-Id", _options.ClientId);

        var response = await _twitchHttpClient.GetAsync("users");
        response.EnsureSuccessStatusCode();

        string responseContent = await response.Content.ReadAsStringAsync();
        var usersResponse = JsonSerializer.Deserialize<TwitchUsersResponse>(responseContent);
        return usersResponse?.Data.FirstOrDefault();

    }

    private async Task<string?> Authenticate(string code)
    {
        using var client = _httpClientFactory.CreateClient();

        // Prepare the form data
        var formData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", _options.ClientId),
                new KeyValuePair<string, string>("client_secret", _options.ClientSecret),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("redirect_uri", _options.CallbackUrl)
            };

        // Create the HTTP request content
        var content = new FormUrlEncodedContent(formData);

        // Send the POST request
        var response = await client.PostAsync(_options.TokenEndpoint, content);

        // Check if the request was successful
        if (response.IsSuccessStatusCode)
        {
            // Read and output the response content (token information)
            string responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);
            return tokenResponse?.AccessToken;
        }

        return null;
    }
}


public class TokenResponse
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


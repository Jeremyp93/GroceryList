using GroceryList.Application.Abstractions;
using GroceryList.Application.Commands.LoginGoogle;
using GroceryList.Application.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GroceryList.Infrastructure.Services;
public class GoogleClient : IGoogleClient
{
    private readonly HttpClient _googleHttpClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly GoogleOptions _options;

    public GoogleClient(HttpClient googleHttpClient, IHttpClientFactory httpClientFactory, IOptions<GoogleOptions> options)
    {
        _googleHttpClient = googleHttpClient ?? throw new ArgumentNullException(nameof(googleHttpClient));
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
    }

    public async Task<GoogleUser?> GetUser(string code)
    {
        var token = await Authenticate(code);

        if (token is null)
        {
            return null;
        }

        // Set headers
        _googleHttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        _googleHttpClient.DefaultRequestHeaders.Add("Client-Id", _options.ClientId);

        var response = await _googleHttpClient.GetAsync("userinfo?alt=json");
        response.EnsureSuccessStatusCode();

        string responseContent = await response.Content.ReadAsStringAsync();
        var usersResponse = JsonSerializer.Deserialize<GoogleUser>(responseContent);
        return usersResponse;
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
            
            var tokenResponse = JsonSerializer.Deserialize<GoogleTokenResponse>(responseContent);
            if (tokenResponse is null)
                return null;

            if (!tokenResponse.IsSuccess)
            {
                throw new Exception(tokenResponse.error_description);
            }
            return tokenResponse.AccessToken;
        }

        return null;
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

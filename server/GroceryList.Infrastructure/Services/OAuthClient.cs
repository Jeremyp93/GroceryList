using GroceryList.Application.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace GroceryList.Infrastructure.Services;
public abstract class OAuthClient<TOptions, TUser, TTokenResponse>
    where TOptions : OAuthOptions
    where TTokenResponse : class
{
    private readonly HttpClient _httpClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly TOptions _options;

    public OAuthClient(HttpClient httpClient, IHttpClientFactory httpClientFactory, IOptions<TOptions> options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
    }

    public async Task<TUser?> GetUser(string code)
    {
        var token = await Authenticate(code);

        if (token is null)
        {
            return default;
        }

        // Set headers
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        _httpClient.DefaultRequestHeaders.Add("Client-Id", _options.ClientId);

        var response = await _httpClient.GetAsync(GetUserInfoEndpoint());
        response.EnsureSuccessStatusCode();

        string responseContent = await response.Content.ReadAsStringAsync();
        return GetUserInfoFromResponse(responseContent);
    }

    protected abstract string GetUserInfoEndpoint();
    protected abstract TUser? GetUserInfoFromResponse(string responseString);

    private async Task<string?> Authenticate(string code)
    {
        using var client = _httpClientFactory.CreateClient();

        var formData = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("client_id", _options.ClientId),
            new KeyValuePair<string, string>("client_secret", _options.ClientSecret),
            new KeyValuePair<string, string>("code", code),
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("redirect_uri", _options.CallbackUrl)
        };

        var content = new FormUrlEncodedContent(formData);
        var response = await client.PostAsync(_options.TokenEndpoint, content);

        if (response.IsSuccessStatusCode)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TTokenResponse>(responseContent);
            return ExtractAccessToken(tokenResponse);
        }

        return null;
    }

    protected abstract string? ExtractAccessToken(TTokenResponse? tokenResponse);
}

using System.Text.Json.Serialization;

namespace GroceryList.Application.Models;
public class TwitchUsersResponse
{
    [JsonPropertyName("data")]
    public IEnumerable<TwitchUser> Data { get; set; } = new List<TwitchUser>();
}

public class TwitchUser
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    [JsonPropertyName("login")]
    public string Login { get; set; } = string.Empty;
    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; } = string.Empty;
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;
    [JsonPropertyName("broadcaster_type")]
    public string BroadcasterType { get; set; } = string.Empty;
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
    [JsonPropertyName("profile_image_url")]
    public string ProfileImageUrl { get; set; } = string.Empty;
    [JsonPropertyName("offline_image_url")]
    public string OfflineImageUrl { get; set; } = string.Empty;
    [JsonPropertyName("view_count")]
    public int ViewCount { get; set; }
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
}

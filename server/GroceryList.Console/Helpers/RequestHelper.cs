using System.Text;
using System.Text.Json;

namespace GroceryList.Console.Helpers;
internal static class RequestHelper
{
    private const string WebApiUrl = "http://localhost:5058/api/";

    //public static async Task<List<T>> GetAll<T>(string endpoint)
    //{
    //    using HttpClient client = new HttpClient();
    //    client.BaseAddress = new Uri(WebApiUrl);

    //    // Setup HttpClient and make a request to the URL
    //    HttpResponseMessage response = await client.GetAsync(endpoint);

    //    // Check if the request was successful (status code 200-299)
    //    if (response.IsSuccessStatusCode)
    //    {
    //        // Read and output the content
    //        string content = await response.Content.ReadAsStringAsync();
    //        var options = new JsonSerializerOptions
    //        {
    //            PropertyNameCaseInsensitive = true
    //        };
    //        var result = JsonSerializer.Deserialize<List<T>>(content, options);
    //        return result;
    //    }
    //    else
    //    {
    //        return new List<T>();
    //    }
    //}

    public static async Task<T> Add<T>(string endpoint, object data)
    {
        using HttpClient client = new();
        client.BaseAddress = new Uri(WebApiUrl);

        string jsonContent = JsonSerializer.Serialize(data);
        var contentData = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        // Setup HttpClient and make a request to the URL
        HttpResponseMessage response = await client.PostAsync(endpoint, contentData);

        // Check if the request was successful (status code 200-299)
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Creation did not work for entity of type ({nameof(data)})");
        }

        string content = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var result = JsonSerializer.Deserialize<T>(content, options);
        return result;
    }
}

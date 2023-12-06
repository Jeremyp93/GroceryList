using GroceryList.Application.Abstractions;
using GroceryList.Application.Queries.Geocode;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GroceryList.Infrastructure.Services
{
    public class GeoapifyClient : IAutoCompleteClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GeoapifyClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration;
        }

        public async Task<IEnumerable<AutocompleteResponse>> AutoComplete(string searchText)
        {
            var a = _httpClient.BaseAddress;
            var apiKey = _configuration.GetValue<string>("AutoComplete:ApiKey") ?? "";
            string apiUrl = $"autocomplete?text={searchText}&apiKey={apiKey}&limit=5";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var result =  await response.Content.ReadAsStringAsync();
                var root = JsonSerializer.Deserialize<GeoapifyResult.Root>(result);
                var addresses = new List<AutocompleteResponse>();

                foreach (var feature in root.Features)
                {
                    var properties = feature.Properties;

                    var address = new AutocompleteResponse
                    {
                        Formatted = properties?.Formatted ?? "",
                        Street = properties?.Street ?? "",
                        ZipCode = properties?.Postcode ?? "",
                        City = properties?.City ?? "",
                        Country = properties?.Country ?? ""
                    };

                    addresses.Add(address);
                }

                return addresses;
            }
            else
            {
                // Handle the error here (e.g., throw an exception)
                return new List<AutocompleteResponse>();
            }
        }
    }

    public class GeoapifyResult
    {
        public class Feature
        {

            [JsonPropertyName("properties")]
            public Properties Properties { get; set; }
        }

        public class Properties
        {

            [JsonPropertyName("country")]
            public string Country { get; set; }

            [JsonPropertyName("postcode")]
            public string Postcode { get; set; }

            [JsonPropertyName("state")]
            public string State { get; set; }

            [JsonPropertyName("city")]
            public string City { get; set; }

            [JsonPropertyName("state_code")]
            public string StateCode { get; set; }

            [JsonPropertyName("formatted")]
            public string Formatted { get; set; }

            [JsonPropertyName("street")]
            public string Street { get; set; }
        }

        public class Root
        {
            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("features")]
            public List<Feature> Features { get; set; }
        }
    }
}

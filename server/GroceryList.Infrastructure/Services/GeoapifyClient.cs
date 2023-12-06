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
        //public class Datasource
        //{
        //    [JsonPropertyName("sourcename")]
        //    public string Sourcename { get; set; }

        //    [JsonPropertyName("url")]
        //    public string Url { get; set; }

        //    [JsonPropertyName("attribution")]
        //    public string Attribution { get; set; }

        //    [JsonPropertyName("license")]
        //    public string License { get; set; }
        //}

        public class Feature
        {
            //[JsonPropertyName("type")]
            //public string Type { get; set; }

            //[JsonPropertyName("geometry")]
            //public Geometry Geometry { get; set; }

            [JsonPropertyName("properties")]
            public Properties Properties { get; set; }

            //[JsonPropertyName("bbox")]
            //public List<double> Bbox { get; set; }
        }

        //public class Geometry
        //{
        //    [JsonPropertyName("type")]
        //    public string Type { get; set; }

        //    [JsonPropertyName("coordinates")]
        //    public List<double> Coordinates { get; set; }
        //}

        public class Properties
        {
            //[JsonPropertyName("country_code")]
            //public string CountryCode { get; set; }

            //[JsonPropertyName("name")]
            //public string Name { get; set; }

            [JsonPropertyName("country")]
            public string Country { get; set; }

            //[JsonPropertyName("datasource")]
            //public Datasource Datasource { get; set; }

            [JsonPropertyName("postcode")]
            public string Postcode { get; set; }

            [JsonPropertyName("state")]
            public string State { get; set; }

            //[JsonPropertyName("district")]
            //public string District { get; set; }

            [JsonPropertyName("city")]
            public string City { get; set; }

            [JsonPropertyName("state_code")]
            public string StateCode { get; set; }

            //[JsonPropertyName("NUTS_1")]
            //public string NUTS1 { get; set; }

            //[JsonPropertyName("lon")]
            //public double Lon { get; set; }

            //[JsonPropertyName("lat")]
            //public double Lat { get; set; }

            [JsonPropertyName("formatted")]
            public string Formatted { get; set; }

            //[JsonPropertyName("address_line1")]
            //public string AddressLine1 { get; set; }

            //[JsonPropertyName("address_line2")]
            //public string AddressLine2 { get; set; }

            //[JsonPropertyName("timezone")]
            //public Timezone Timezone { get; set; }

            //[JsonPropertyName("plus_code")]
            //public string PlusCode { get; set; }

            //[JsonPropertyName("plus_code_short")]
            //public string PlusCodeShort { get; set; }

            //[JsonPropertyName("result_type")]
            //public string ResultType { get; set; }

            //[JsonPropertyName("rank")]
            //public Rank Rank { get; set; }

            //[JsonPropertyName("place_id")]
            //public string PlaceId { get; set; }

            //[JsonPropertyName("region")]
            //public string Region { get; set; }

            //[JsonPropertyName("county")]
            //public string County { get; set; }

            //[JsonPropertyName("hamlet")]
            //public string Hamlet { get; set; }

            //[JsonPropertyName("municipality")]
            //public string Municipality { get; set; }

            [JsonPropertyName("street")]
            public string Street { get; set; }

            //[JsonPropertyName("state_COG")]
            //public string StateCOG { get; set; }

            //[JsonPropertyName("department_COG")]
            //public string DepartmentCOG { get; set; }
        }

        //public class Query
        //{
        //    [JsonPropertyName("text")]
        //    public string Text { get; set; }
        //}

        //public class Rank
        //{
        //    [JsonPropertyName("confidence")]
        //    public int Confidence { get; set; }

        //    [JsonPropertyName("confidence_city_level")]
        //    public int ConfidenceCityLevel { get; set; }

        //    [JsonPropertyName("match_type")]
        //    public string MatchType { get; set; }

        //    [JsonPropertyName("importance")]
        //    public double? Importance { get; set; }
        //}

        public class Root
        {
            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("features")]
            public List<Feature> Features { get; set; }

            //[JsonPropertyName("query")]
            //public Query Query { get; set; }
        }

        //public class Timezone
        //{
        //    [JsonPropertyName("name")]
        //    public string Name { get; set; }

        //    [JsonPropertyName("name_alt")]
        //    public string NameAlt { get; set; }

        //    [JsonPropertyName("offset_STD")]
        //    public string OffsetSTD { get; set; }

        //    [JsonPropertyName("offset_STD_seconds")]
        //    public int OffsetSTDSeconds { get; set; }

        //    [JsonPropertyName("offset_DST")]
        //    public string OffsetDST { get; set; }

        //    [JsonPropertyName("offset_DST_seconds")]
        //    public int OffsetDSTSeconds { get; set; }

        //    [JsonPropertyName("abbreviation_STD")]
        //    public string AbbreviationSTD { get; set; }

        //    [JsonPropertyName("abbreviation_DST")]
        //    public string AbbreviationDST { get; set; }
        //}
    }
}

using System.Text.Json;

namespace ColruytScraper;
public class ColruytClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ColruytClient> _logger;
    private const string ColruytPlaceID = "597"; //Kraainem
    private const int PageSize = 250;

    public ColruytClient(HttpClient httpClient, ILogger<ColruytClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IEnumerable<ColruytProduct>> GetAllProducts(int percentageRequiredOutOf100)
    {
        var apiKey = await ColruytApiHelper.GetXCGAPIKey(_logger);
        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogError("API key not found");
            return [];
        }

        var allProducts = new List<ColruytProduct>();

        var productsResponse = await DoAPICall(1, PageSize, apiKey);

        if (productsResponse is null)
        {
            _logger.LogError("Failed to deserialize products response");
            return [];
        }

        var totalProductsFound = productsResponse.ProductsFound;
        int totalPages = (totalProductsFound / PageSize) + 1;

        // Define the total number of products required based on percentage
        double percentageRequired = percentageRequiredOutOf100 / 100.0;
        int productsRequired = (int)(totalProductsFound * percentageRequired);

        _logger.LogInformation($"Should retrieve at least {productsRequired} products out of {totalProductsFound} ({percentageRequiredOutOf100}%)");

        // Add unique products
        allProducts.AddRange(ToColruytProducts(productsResponse.Products));

        // Loop through pages to retrieve products
        for (int page = 2; page <= totalPages; page++)
        {
            productsResponse = await DoAPICall(page, PageSize, apiKey);

            if (productsResponse is null)
            {
                _logger.LogError("Failed to retrieve products response");
                return [];
            }

            // Add unique products
            allProducts.AddRange(ToColruytProducts(productsResponse.Products));

            _logger.LogInformation($"Page {page}/{totalPages}: Retrieved {allProducts.Count}/{productsRequired} products");

            // Break if we have reached the required number of products
            if (allProducts.Count >= productsRequired)
            {
                _logger.LogInformation("Enough products retrieved, stopping...");
                break;
            }

            await Task.Delay(5000);
        }

        return allProducts;
    }

    private async Task<ProductsResponse?> DoAPICall(int page, int size, string xCgApiKey)
    {
        try
        {
            var url = "products";
            url += $"?clientCode=CLP";
            url += $"&page={page}";
            url += $"&size={PageSize}";
            url += $"&placeId={ColruytPlaceID}";
            url += $"&sort=popularity+asc";
            url += $"&isAvailable=true";
            var requestUrl = $"{_httpClient.BaseAddress?.AbsoluteUri}{url}";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("X-Cg-Apikey", xCgApiKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProductsResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during API call: {ex.Message}");
            // Implement retry mechanism as needed, similar to Go code.
            return await Retry(page, size, xCgApiKey);
        }
    }

    private async Task<ProductsResponse?> Retry(int page, int size, string xCgApiKey)
    {
        // Implement retry logic
        _logger.LogInformation("Retrying...");
        await Task.Delay(10000);
        return await DoAPICall(page, size, xCgApiKey);
    }

    private IEnumerable<ColruytProduct> ToColruytProducts(IEnumerable<Product> products)
    {
        return products.Select(p =>
        {
            var product = new ColruytProduct
            {
                Price = p.Price?.BasicPrice ?? 0,
                Name = p.LongName,
                Image = p.FullImage,
                Thumbnail = p.SquareImage,
                ProductId = p.ProductId,
                Id = Guid.NewGuid()
            };
            if (p.Categories is not null && p.Categories.Count != 0)
            {
                var category = p.Categories.First();
                product.Category = new ColruytCategory { Name = category.Name, Id = category.Id };
            }
            return product;
        });
    }
}

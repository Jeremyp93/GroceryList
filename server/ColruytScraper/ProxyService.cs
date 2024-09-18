using System.Net;

namespace ColruytScraper;
public class ProxyService
{
    private readonly HttpClient _httpClient;
    private readonly List<WebProxy> _proxies;

    public ProxyService(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient.CreateClient();
        _proxies = new List<WebProxy>();
    }

    private async Task FetchProxiesAsync()
    {
        var proxyApiUrl = "https://api.proxyscrape.com/v4/free-proxy-list/get?request=display_proxies&country=be,fr&proxy_format=protocolipport&format=text&timeout=20000";

        var response = await _httpClient.GetStringAsync(proxyApiUrl);
        var proxyLines = response.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        _proxies.Clear();
        foreach (var line in proxyLines)
        {
            var proxy = new WebProxy(line);
            _proxies.Add(proxy);
        }
    }

    public async Task<List<WebProxy>> GetProxiesAsync()
    {
        if (_proxies.Count == 0)
        {
            await FetchProxiesAsync();
        }
        return _proxies;
    }
}
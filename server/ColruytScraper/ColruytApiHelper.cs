using PuppeteerSharp;

namespace ColruytScraper
{
    internal static class ColruytApiHelper
    {
        internal static async Task<string?> GetXCGAPIKey(ILogger logger)
        {
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync(BrowserTag.Stable);

            var installedBrowser = await new BrowserFetcher().DownloadAsync(BrowserTag.Stable);

            var options = new LaunchOptions
            {
                Headless = true, // Set to true for headless mode
                ExecutablePath = installedBrowser.GetExecutablePath(),
                Args = new[] { "--no-sandbox" }
            };

            using var browser = await Puppeteer.LaunchAsync(options);
            var page = await browser.NewPageAsync();
            await page.SetUserAgentAsync("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36 OPR/107.0.0.0");

            // Enable request interception
            await page.SetRequestInterceptionAsync(true);

            string apiKeyFromRequest = null;

            // Handle request events
            page.Request += async (sender, e) =>
            {
                if (e.Request.Headers.TryGetValue("X-CG-APIKey", out string? value))
                {
                    var apiKeyHeader = value;
                    if (!string.IsNullOrEmpty(apiKeyHeader))
                    {
                        logger.LogInformation($"API key {apiKeyHeader} found on URL {e.Request.Url}");
                        apiKeyFromRequest = apiKeyHeader;
                    }
                }

                await e.Request.ContinueAsync();
            };

            await page.GoToAsync("https://colruyt.be/nl", new NavigationOptions
            {
                WaitUntil = new[] { WaitUntilNavigation.DOMContentLoaded },
                Timeout = 120000
            });

            int loopsToGo = 18; // Increase wait time to 2 minutes (24 * 5 seconds)
            while (loopsToGo-- > 0)
            {
                if (!string.IsNullOrEmpty(apiKeyFromRequest))
                {
                    return apiKeyFromRequest;
                }
                logger.LogInformation("Still waiting for API key...");
                await Task.Delay(5000); // 5 seconds delay
            }

            logger.LogInformation("No API key found after 90 seconds.");
            return null;
        }
    }
}

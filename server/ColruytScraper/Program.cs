using ColruytScraper;

var builder = Host.CreateApplicationBuilder(args);

var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.Development.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

builder.Services.AddHostedService<Worker>();
builder.Services.ConfigureMongoDb(configuration);

builder.Services.AddSingleton<ColruytClient>();
builder.Services.AddHttpClient<ColruytClient>(colruytClient =>
{
    colruytClient.BaseAddress = new Uri("https://apip.colruyt.be/gateway/emec.colruyt.protected.bffsvc/cg/fr/api/");
    colruytClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36 OPR/107.0.0.0");
});


var host = builder.Build();
host.Run();

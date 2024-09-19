using MongoDB.Driver;

namespace ColruytScraper;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ColruytClient _colruytClient;
    private readonly IMongoDatabase _database;

    public Worker(ILogger<Worker> logger, ColruytClient colruytClient, IMongoDatabase database)
    {
        _logger = logger;
        _colruytClient = colruytClient;
        _database = database;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            var result = await _colruytClient.GetAllProducts(100);

            if (result.Any())
            {
                await SaveToMongoDbAsync(result);
            }

            Environment.Exit(0);
        }
    }

    private async Task SaveToMongoDbAsync(IEnumerable<ColruytProduct> products)
    {
        var collection = _database.GetCollection<ColruytProduct>("colruyt_data");

        await collection.DeleteManyAsync(Builders<ColruytProduct>.Filter.Empty);
        _logger.LogInformation("All products deleted from DB");
        // Insert into MongoDB collection
        await collection.InsertManyAsync(products);
        _logger.LogInformation("All products added to DB");
    }
}

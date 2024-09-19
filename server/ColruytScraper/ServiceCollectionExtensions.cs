using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace ColruytScraper;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var pack = new ConventionPack
                    {
                        new CamelCaseElementNameConvention()
                    };

        ConventionRegistry.Register("camelCase", pack, t => true);

        var mongoDbSettings = configuration.GetSection("MongoDb");
        var mongoDbConn = string.Format(configuration.GetConnectionString("MongoDBConnection") ?? "", mongoDbSettings.GetValue<string>("Username"), mongoDbSettings.GetValue<string>("Password"));

        services.AddSingleton<IMongoDatabase>(serviceProvider =>
        {
            MongoClient mongoClient = new(mongoDbConn);
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase("grocery_list");

            return mongoDatabase;
        });

        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));

        return services;
    }
}

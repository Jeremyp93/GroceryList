using GroceryList.Application;
using GroceryList.Application.Abstractions;
using GroceryList.Domain.Events;
using GroceryList.Domain.Helpers;
using GroceryList.Domain.Helpers.Contracts;
using GroceryList.Domain.Repositories;
using GroceryList.Infrastructure.Authentication;
using GroceryList.Infrastructure.DataBase;
using GroceryList.Infrastructure.Repositories;
using GroceryList.WebApi.OptionsSetup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace GroceryList.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Program));

        return services;
    }

    public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IStoreRepository, StoreRepository>();
        services.AddTransient<IGroceryListRepository, GroceryListRepository>();

        return services;
    }

    public static IServiceCollection ConfigureHelpers(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();

        return services;
    }

    public static IServiceCollection ConfigureMongoDb(this IServiceCollection services, ILogger logger)
    {
        var pack = new ConventionPack
        {
            new CamelCaseElementNameConvention()
        };

        ConventionRegistry.Register("camelCase", pack, t => true);

        services.AddSingleton<IMongoDatabase>(serviceProvider =>
        {
            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var mongoDbSettings = configuration.GetSection("MongoDb");
            var connectionString = string.Format(configuration.GetConnectionString("MongoDBConnection") ?? "", mongoDbSettings.GetValue<string>("Username"), mongoDbSettings.GetValue<string>("Password"));
            logger.LogInformation(connectionString);
            MongoClient mongoClient = new MongoClient(connectionString);
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase("grocery_list");

            return mongoDatabase;
        });
        return services;
    }


    public static IServiceCollection ConfigureMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ResultStatusCode).Assembly));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IDomainEvent).Assembly));

        return services;
    }

    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAnyOrigin",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowScotexTech",
                builder => builder
                    .WithOrigins("https://grocery-list.scotex.tech")
                    .AllowAnyHeader()
                    .WithMethods("GET", "POST", "PUT", "DELETE"));
        });

        return services;
    }

    public static IServiceCollection SetupAuthentication(this IServiceCollection services)
    {
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        services.AddHttpContextAccessor();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IClaimReader, ClaimReader>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
        return services;
    }
}

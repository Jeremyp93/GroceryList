using GroceryList.Application;
using GroceryList.Application.Abstractions;
using GroceryList.Application.Commands.LoginTwitch;
using GroceryList.Domain.Events;
using GroceryList.Domain.Helpers;
using GroceryList.Domain.Helpers.Contracts;
using GroceryList.Domain.Repositories;
using GroceryList.Infrastructure.Authentication;
using GroceryList.Infrastructure.Repositories;
using GroceryList.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using MongoDB.Bson.Serialization.Conventions;
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
        services.AddTransient<IItemRepository, ItemRepository>();

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
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowScotexTech",
                builder => builder
                    .WithOrigins("https://grocery-list.scotex.tech")
                    .AllowAnyHeader()
                    .WithMethods("GET", "POST", "PUT", "DELETE")
                    .AllowCredentials());
        });

        return services;
    }

    public static IServiceCollection SetupCookieAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<TwitchOptions>().Bind(configuration.GetSection("twitch"));
        services.AddHttpContextAccessor();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IClaimReader, ClaimReader>();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "AuthCookie";
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.Domain = configuration.GetValue<string>("Cookie:Domain");
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);

                if (configuration.GetValue<bool>("IsProduction"))
                {
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                }

                options.Events.OnRedirectToLogin = (context) =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

        return services;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        // Add HttpClient and configure its base address
        services.AddHttpClient<IAutoCompleteClient, GeoapifyClient>(client =>
        {
            client.BaseAddress = new Uri("https://api.geoapify.com/v1/geocode/");
        });

        // Add HttpClient and configure its base address
        services.AddHttpClient<ITwitchClient, TwitchClient>(twithClient =>
        {
            twithClient.BaseAddress = new Uri("https://api.twitch.tv/helix/");
        });

        return services;
    }
}

using GroceryList.WebApi.Extensions;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Logging
  .ClearProviders()
  .AddConsole();


builder.Services.AddControllersWithViews();

using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
    .SetMinimumLevel(LogLevel.Trace)
    .AddConsole());

ILogger logger = loggerFactory.CreateLogger<Program>();
logger.LogInformation("Example log message");

builder.Services.ConfigureAutoMapper();
builder.Services.ConfigureMediatR();
builder.Services.ConfigureHelpers();
builder.Services.ConfigureRepositories();
builder.Services.ConfigureCors();
builder.Services.ConfigureMongoDb(logger);
builder.Services.SetupAuthentication();
builder.Services.ConfigureServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Grocery List API", Version = "v1" });
//});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Grocery List API", Version = "v1" });

    // Add bearer token authentication in Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
});

builder.Services.AddControllers();

/* builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 443; // Set the HTTPS port
}); */

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Grocery List API V1");
    });
}

if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAnyOrigin");
}
else
{
    app.UseCors("AllowScotexTech");
}

/* app.UseHttpsRedirection(); */

app.UseAuthentication();

app.MapControllers();

app.UseRouting();

app.UseAuthorization();

app.Run();

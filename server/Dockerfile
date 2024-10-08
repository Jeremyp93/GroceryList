FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Copy everything
# WORKDIR /src
COPY ./GroceryList.sln ./
COPY ./GroceryList.WebApi/GroceryList.WebApi.csproj  ./GroceryList.WebApi/GroceryList.WebApi.csproj
COPY ./GroceryList.Application/GroceryList.Application.csproj  ./GroceryList.Application/GroceryList.Application.csproj
COPY ./GroceryList.Domain/GroceryList.Domain.csproj  ./GroceryList.Domain/GroceryList.Domain.csproj
COPY ./GroceryList.Infrastructure/GroceryList.Infrastructure.csproj  ./GroceryList.Infrastructure/GroceryList.Infrastructure.csproj

# Restore as distinct layers
RUN dotnet restore "GroceryList.WebApi/GroceryList.WebApi.csproj"
COPY ./GroceryList.WebApi/ ./GroceryList.WebApi/
COPY ./GroceryList.Application/ ./GroceryList.Application/
COPY ./GroceryList.Domain/ ./GroceryList.Domain/
COPY ./GroceryList.Infrastructure/ ./GroceryList.Infrastructure/
# Build and publish a release
RUN dotnet publish "GroceryList.WebApi/GroceryList.WebApi.csproj" -c Release -o out
# RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "GroceryList.WebApi.dll"]
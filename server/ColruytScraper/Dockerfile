# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["ColruytScraper.csproj", "./"]
RUN dotnet restore "./ColruytScraper.csproj"

# Copy the remaining source code and build
COPY . .
RUN dotnet publish "ColruytScraper.csproj" -c Release -o /app/publish

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "ColruytScraper.dll"]
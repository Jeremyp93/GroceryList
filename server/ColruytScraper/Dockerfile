# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["ColruytScraper.csproj", "./"]
RUN dotnet restore "./ColruytScraper.csproj"

# Copy the remaining source code and build
COPY . .
RUN dotnet publish "ColruytScraper.csproj" -c Release -o /app/publish

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/runtime:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

RUN apt-get update && apt-get install -y xorg openbox libnss3 libasound2

# Set the entry point for the container
ENTRYPOINT ["dotnet", "ColruytScraper.dll"]
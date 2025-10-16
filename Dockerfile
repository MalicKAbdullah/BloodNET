# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["BloodNET-Web/BloodNET-Web.csproj", "BloodNET-Web/"]
RUN dotnet restore "BloodNET-Web/BloodNET-Web.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/BloodNET-Web"
RUN dotnet build "BloodNET-Web.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "BloodNET-Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install curl for healthchecks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .

# Set environment variable for production
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "BloodNET-Web.dll"]

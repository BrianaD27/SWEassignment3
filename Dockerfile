# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["SWEASSIGNMENT3.sln", "./"]
COPY ["DBConnectorLibrary/", "DBConnectorLibrary/"]
COPY ["TestDBConnector/", "TestDBConnector/"]
COPY ["DBConnectorConsole/", "DBConnectorConsole/"]

# Restore and build
RUN dotnet restore "SWEASSIGNMENT3.sln"
RUN dotnet build "SWEASSIGNMENT3.sln" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "DBConnectorConsole/DBConnectorConsole.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "DBConnectorConsole.dll"]
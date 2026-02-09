# Hacker News API Application

A .NET 10 ASP.NET Core API application that fetches and serves stories from the Hacker News API with caching and resilience patterns.

## How to Run the Application

### Prerequisites

- .NET 10 SDK installed
- Visual Studio 2022 (or any compatible C# IDE), or the .NET CLI

### Build

From the repository src folder, run:

```bash
dotnet restore
dotnet build -c Release
```

### Run

To start the API:

```bash
dotnet run --project HackerNews.Host
```

Or run the Host project (if configured as the startup project):

```bash
dotnet run --project HackerNews.Host
```

The API will be available at `http://localhost:5181`.
The Api documentation will be available at `http://localhost:5181/scalar/v1`

### Run Tests

To execute all unit tests:

```bash
dotnet test
```

To run a specific test project:

```bash
dotnet test HackerNews.Core.Tests
```

## Project Structure

- **HackerNews.Contract**: Project contract Data transfer objects (DTOs) like `Story`
- **HackerNews.Core**: Core abstractions including:
  - **Modularity**: Module manager for plugin-based architecture
  - **Caching**: `IObjectCache`, `MemoryObjectCache`, `DistributedObjectCache` abstractions
- **HackerNews.Data**: Data access layer
  - `HackerNewsApiClient`: HTTP client for fetching stories from the Hacker News API
  - `PollyPolicies`: Resilience policies (retry, timeout, circuit breaker)
- **HackerNews.Logic**: Business logic layer
  - `IBestStoriesService`: Service for retrieving best stories
  - `CachedHackerNewsApiClient`: Decorator for caching API responses
- **HackerNews.Api**: ASP.NET Core API controllers (e.g., `StoriesController`)
- **HackerNews.Host**: Application host/startup
- **HackerNews.Core.Tests**: Unit tests for core components


## Architecture Highlights

1. **Modular Design**: The `ModuleManager` enables plugin-based loading of modules per scope (e.g., "api", "host")
2. **Caching Layer**: Abstraction for multiple cache implementations (memory, distributed)
3. **Resilience**: Polly policies applied to external HTTP calls for retry and timeout handling (circuit breaker and retry policies applied)
4. **Dependency Injection**: Full use of Microsoft.Extensions.DependencyInjection
5. **Unit Testing**: Test infrastructure using MSTest and Moq

## Assumptions Made

1. **Network Access**: The application requires internet access to call the real Hacker News API (`https://hacker-news.firebaseio.com/`)
2. **No Authentication**: The Hacker News API is public; no API keys or authentication headers are required
3. **.NET 10 Runtime**: The solution targets .NET 10; ensure the SDK is installed
4. **Development Machine**: Built and tested on a standard developer workstation; no special infrastructure required
5. **Caching Backend**: In-memory caching is the default; distributed caching (Redis, etc.) would require configuration and an external service


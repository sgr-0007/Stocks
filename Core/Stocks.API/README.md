# Stocks API

A containerized .NET 8 Web API for managing stocks and comments, deployed to Azure using Docker and GitHub Actions. Includes comprehensive unit testing with mock repositories.

## Architecture

This application follows a cloud-native architecture using Azure services:

- **Web API**: ASP.NET Core 8.0 REST API with Swagger documentation
- **Database**: Azure SQL Database for data persistence
- **Containerization**: Docker multi-stage build process
- **CI/CD**: GitHub Actions workflow for automated deployment
- **Cloud Hosting**: Azure Web App for Containers
- **Testing**: xUnit with mock repositories for isolated unit testing

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
- [GitHub Account](https://github.com/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/) (recommended)


## Configuration Setup

1. For local development, add your connection string to `appsettings.json` or use User Secrets:

```json
{
  "ConnectionStrings": {
      "DefaultConnection": "Server=yourserveraddr;Initial Catalog=yourDb;Persist Security Info=False;User ID=youruserid;Password=pass$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
}
```

## Local Development

### Running with Docker Compose

```bash
# Build and start the application with its dependencies
docker-compose up --build

# The API will be available at http://localhost:5050
```

### Running without Docker

```bash
# Restore dependencies
dotnet restore

# Run the application
dotnet run

# The API will be available at https://localhost:5001
```

## Database Migrations

This project uses Entity Framework Core for database migrations:

```bash
# Add a new migration
dotnet ef migrations add MigrationName

# Apply migrations to the database
dotnet ef database update
```

## API Documentation

Swagger UI is available:

- Development: https://localhost:5050/swagger

### API Endpoints

#### Stocks API

- `GET /api/v1/stocks` - Get all stocks with optional filtering, sorting, and pagination
- `GET /api/v1/stocks/{id}` - Get stock by ID
- `POST /api/v1/stocks` - Create a new stock
- `PUT /api/v1/stocks/{id}` - Update an existing stock
- `DELETE /api/v1/stocks/{id}` - Delete a stock

#### Comments API

- `GET /api/v1/comments` - Get all comments
- `GET /api/v1/comments/{id}` - Get comment by ID
- `POST /api/v1/comments` - Create a new comment
- `PUT /api/v1/comments/{id}` - Update an existing comment
- `DELETE /api/v1/comments/{id}` - Delete a comment

## Azure Deployment

The application is deployed to Azure using GitHub Actions:

1. **Azure Resources**:
   - Resource Group: `stocks-api-rg`
   - App Service Plan: `stocks-api-plan`
   - Web App for Containers: `stocks-api-webapp`
   - Azure Container Registry: `stocksapiregistry`
   - Azure SQL Server: `stocksapi-sql`
   - Azure SQL Database: `Stocks`

2. **GitHub Secrets**:
   - `AZURE_CREDENTIALS`: Service principal credentials for Azure
   - `REGISTRY_LOGIN_SERVER`: ACR login server URL
   - `REGISTRY_USERNAME`: ACR username
   - `REGISTRY_PASSWORD`: ACR password
   - `DB_CONNECTION_STRING`: Azure SQL connection string

## CI/CD Pipeline

The GitHub Actions workflow in `.github/workflows/deploy.yml` performs:

1. Build and test the application
2. Run database migrations
3. Build and push Docker image to ACR
4. Deploy to Azure Web App for Containers

## Testing

This project uses a comprehensive testing approach with xUnit and FluentAssertions. All tests are integrated into the CI/CD pipeline to ensure code quality:

### Repository Tests

Repository tests use mock implementations instead of EF Core InMemory database to avoid issues with required properties and provide better isolation:

- **MockStockRepository**: In-memory implementation of IStockRepository
  - Simulates operations for stocks
  - Supports filtering, sorting, and pagination
  - Maintains consistent test data across test runs

- **MockCommentRepository**: In-memory implementation of ICommentRepository
  - Simulates operations for comments
  - Maintains referential integrity with stocks

### Controller Tests

Controller tests use mock repositories to test API endpoints in isolation.

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --logger "console;verbosity=detailed"
```

### Test Structure

- **Test/Repository/**: Tests for data repositories
  - StockRepositoryTests.cs: Tests for stock operations, filtering, sorting, and pagination
  - CommentRepositoryTests.cs: Tests for comment operations
- **Test/Controllers/**: Tests for API controllers
  - StockControllerTests.cs: Tests for stock API endpoints
  - CommentControllerTests.cs: Tests for comment API endpoints
- **Test/Mocks/**: Mock implementations for testing
  - MockStockRepository.cs: In-memory implementation of IStockRepository
  - MockCommentRepository.cs: In-memory implementation of ICommentRepository

## Project Structure

```
Stocks.API/
├── Controllers/         # API controllers
│   ├── V1/             # Version 1 API endpoints
│   │   ├── CommentController.cs
│   │   └── StockController.cs
├── Data/               # Data access layer
│   ├── ApplicationDBContext.cs
│   └── Migrations/     # EF Core migrations
├── Dtos/               # Data Transfer Objects
│   ├── Comment/        # Comment DTOs
│   └── Stock/          # Stock DTOs
├── Helpers/            # Helper classes
│   └── QueryObject.cs  # Query parameters for filtering/sorting
├── Interfaces/         # Repository interfaces
│   ├── ICommentRepository.cs
│   └── IStockRepository.cs
├── Mappers/            # Object mappers
│   ├── CommentMappers.cs
│   └── StockMappers.cs
├── Models/             # Domain models
│   ├── Comment.cs
│   └── Stock.cs
└── Repository/         # Repository implementations
    ├── CommentRepository.cs
    └── StockRepository.cs
```

- `/Controllers`: API endpoints for stocks and comments
- `/Models`: Entity models for database
- `/Data`: DbContext and database configuration
- `/Repository`: Data access layer
- `/Interfaces`: Service interfaces
- `/Dtos`: Data transfer objects
- `/Mappers`: Object mapping extensions
- `/Helpers`: Helper classes

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/a-feature`)
3. Commit your changes (`git commit -m 'a feature'`)
4. Push to the branch (`git push origin feature/a-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

# Stocks API

A containerized .NET 8 Web API for managing stocks and comments, deployed to Azure using Docker and GitHub Actions.

## Architecture

This application follows a cloud-native architecture using Azure services:

- **Web API**: ASP.NET Core 8.0 REST API with Swagger documentation
- **Database**: Azure SQL Database for data persistence
- **Containerization**: Docker multi-stage build process
- **CI/CD**: GitHub Actions workflow for automated deployment
- **Cloud Hosting**: Azure Web App for Containers

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)
- [GitHub Account](https://github.com/)

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

Swagger UI is available

- Development: https://localhost:5050/swagger

## Azure Deployment

The application is deployed to Azure using GitHub Actions:

1. **Azure Resources**:
   - Resource Group: `stocks-api-rg`
   - App Service Plan: `stocks-api-plan`
   - Web App for Containers: `stocks-api-webapp`
   - Azure Container Registry: `stocksapiregistry`
   - Azure SQL Server: `stocksapi-sql`
   - Azure SQL Database: `StocksDb`

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

## Project Structure

- `/Controllers`: API endpoints for stocks and comments
- `/Models`: Entity models for database
- `/Data`: DbContext and database configuration
- `/Repository`: Data access layer
- `/Interfaces`: Service interfaces
- `/Dtos`: Data transfer objects
- `/Mappers`: Object mapping extensions

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/a-feature`)
3. Commit your changes (`git commit -m 'a feature'`)
4. Push to the branch (`git push origin feature/a-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

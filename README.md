# Stocks Management System

A comprehensive stock management solution built with .NET 8, featuring a containerized API for managing stocks and comments.

## Project Overview

This project is a cloud-native application designed to manage stock information and associated comments. It follows modern software architecture principles and is containerized for easy deployment.

## Repository Structure

- **Core/**: Contains the main application code
  - **Stocks.API/**: The main REST API service for stock management
- **Test/**: Contains test projects for the application
  - **Controllers/**: Unit tests for API controllers
  - **Repository/**: Unit tests for data repositories
  - **Mocks/**: Mock implementations for testing
- **docker-compose.yml**: Docker Compose configuration for local development
- **.github/**: GitHub Actions workflows for CI/CD

## Technology Stack

- **Backend**: ASP.NET Core 8.0
- **Database**: Azure SQL Database
- **Containerization**: Docker
- **CI/CD**: GitHub Actions
- **Cloud Hosting**: Azure Web App for Containers
- **Testing**: xUnit, FluentAssertions, Mock Repositories

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) (for deployment)

### Local Development

#### Using Docker Compose

```bash
# Create the required network
docker network create appnet

# Build and start the application with its dependencies
docker-compose up --build

# The API will be available at http://localhost:5050
```

#### Without Docker

```bash
# Navigate to the API project
cd Core/Stocks.API

# Restore dependencies
dotnet restore

# Run the application
dotnet run

# The API will be available at https://localhost:5001
```

## API Documentation

Swagger UI is available for API documentation:

- Development: http://localhost:5050/swagger

## Deployment

The application is configured for deployment to Azure using GitHub Actions. The workflow is defined in `.github/workflows/deploy.yml`.

## Features

- Stock management (create, read, update, delete)
- Comment management for stocks
- Containerized deployment
- CI/CD pipeline
- Swagger documentation
- Comprehensive unit testing

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/feature-name`)
3. Commit your changes (`git commit -m ' feature-name description'`)
4. Push to the branch (`git push origin feature/feature-name`)
5. Ensure all tests pass (`dotnet test`)
6. Open a Pull Request

## Testing

The project uses a comprehensive testing approach:

- **Unit Tests**: Testing individual components in isolation
- **Mock Repositories**: In-memory implementations of repository interfaces for testing
- **Controller Tests**: Testing API endpoints with mock services

```bash
# Run all tests
cd /path/to/Stocks
dotnet test

# Run tests with detailed output
dotnet test --logger "console;verbosity=detailed"
```

## License

This project is licensed under the MIT License - see the LICENSE file in the Core/Stocks.API directory for details.

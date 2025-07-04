using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Stocks.API.Controllers.V1;
using Stocks.API.Dtos.Stock;
using Stocks.API.Helpers;
using Stocks.API.Interfaces;
using Stocks.API.Models;
using Xunit;

namespace Test.Controllers.V1
{
    public class StockControllerTests
    {
        private readonly Mock<IStockRepository> _mockStockRepo;
        private readonly StockController _controller;

        public StockControllerTests()
        {
            _mockStockRepo = new Mock<IStockRepository>();
            _controller = new StockController(_mockStockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfStockDtos()
        {
            // Arrange
            var queryObject = new QueryObject();
            var stocks = new List<Stock>
            {
                new Stock
                {
                    Id = 1,
                    Symbol = "AAPL",
                    CompanyName = "Apple",
                    Purchase = 150.50m,
                    LastDiv = 0.5m,
                    Industry = "Tech",
                    MarketCap = 2000000000,
                    Comments = new List<Comment>()
                },
                new Stock
                {
                    Id = 2,
                    Symbol = "MSFT",
                    CompanyName = "Microsoft",
                    Purchase = 250.75m,
                    LastDiv = 0.6m,
                    Industry = "Tech",
                    MarketCap = 1800000000,
                    Comments = new List<Comment>()
                }
            };

            _mockStockRepo.Setup(repo => repo.GetAllAsync(queryObject))
                .ReturnsAsync(stocks);

            // Act
            var result = await _controller.GetAll(queryObject);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var stockDtos = okResult.Value.Should().BeAssignableTo<IEnumerable<StockDto>>().Subject;
            stockDtos.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetById_WithValidId_ReturnsOkResult_WithStockDto()
        {
            // Arrange
            int stockId = 1;
            var stock = new Stock
            {
                Id = stockId,
                Symbol = "AAPL",
                CompanyName = "Apple",
                Purchase = 150.50m,
                LastDiv = 0.5m,
                Industry = "Tech",
                MarketCap = 2000000000,
                Comments = new List<Comment>()
            };

            _mockStockRepo.Setup(repo => repo.GetByIdAsync(stockId))
                .ReturnsAsync(stock);

            // Act
            var result = await _controller.GetById(stockId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var stockDto = okResult.Value.Should().BeAssignableTo<StockDto>().Subject;
            stockDto.Id.Should().Be(stockId);
            stockDto.Symbol.Should().Be("AAPL");
        }

        [Fact]
        public async Task GetById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            int stockId = 999;
            _mockStockRepo.Setup(repo => repo.GetByIdAsync(stockId))
                .ReturnsAsync((Stock?)null);

            // Act
            var result = await _controller.GetById(stockId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Create_WithValidData_ReturnsCreatedAtAction_WithStockDto()
        {
            // Arrange
            var createDto = new CreateStockRequestDto
            {
                Symbol = "GOOG",
                CompanyName = "Google",
                Purchase = 2000.00m,
                LastDiv = 0.8m,
                Industry = "Tech",
                MarketCap = 1500000000
            };

            var createdStock = new Stock
            {
                Id = 3,
                Symbol = "GOOG",
                CompanyName = "Google",
                Purchase = 2000.00m,
                LastDiv = 0.8m,
                Industry = "Tech",
                MarketCap = 1500000000,
                Comments = new List<Comment>()
            };

            _mockStockRepo.Setup(repo => repo.CreateAsync(It.IsAny<Stock>()))
                .Callback<Stock>(stock => stock.Id = 3)
                .ReturnsAsync(createdStock);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var createdAtActionResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdAtActionResult.ActionName.Should().Be("GetbyId");
            createdAtActionResult.RouteValues.Should().ContainKey("id").And.ContainValue(3);
            var stockDto = createdAtActionResult.Value.Should().BeAssignableTo<StockDto>().Subject;
            stockDto.Symbol.Should().Be("GOOG");
        }

        [Fact]
        public async Task Update_WithValidIdAndData_ReturnsOkResult_WithUpdatedStockDto()
        {
            // Arrange
            int stockId = 1;
            var updateDto = new UpdateStockRequestDto
            {
                Symbol = "AAPL",
                CompanyName = "Apple Inc",
                Purchase = 160.00m,
                LastDiv = 0.6m,
                Industry = "Tech",
                MarketCap = 2100000000
            };

            var updatedStock = new Stock
            {
                Id = stockId,
                Symbol = "AAPL",
                CompanyName = "Apple Inc",
                Purchase = 160.00m,
                LastDiv = 0.6m,
                Industry = "Tech",
                MarketCap = 2100000000,
                Comments = new List<Comment>()
            };

            _mockStockRepo.Setup(repo => repo.UpdateAsync(stockId, updateDto))
                .ReturnsAsync(updatedStock);

            // Act
            var result = await _controller.Update(stockId, updateDto);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var stockDto = okResult.Value.Should().BeAssignableTo<StockDto>().Subject;
            stockDto.Id.Should().Be(stockId);
            stockDto.CompanyName.Should().Be("Apple Inc");
        }

        [Fact]
        public async Task Update_WithInvalidId_ReturnsOkResultWithNull()
        {
            // Arrange
            int stockId = 999;
            var updateDto = new UpdateStockRequestDto
            {
                Symbol = "AAPL",
                CompanyName = "Apple Inc",
                Purchase = 160.00m,
                LastDiv = 0.6m,
                Industry = "Tech",
                MarketCap = 2100000000
            };

            _mockStockRepo.Setup(repo => repo.UpdateAsync(stockId, updateDto))
                .ReturnsAsync((Stock?)null);

            // Act
            var result = await _controller.Update(stockId, updateDto);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeNull();
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsNoContent()
        {
            // Arrange
            int stockId = 1;
            var stock = new Stock { Id = stockId };

            _mockStockRepo.Setup(repo => repo.DeleteAsync(stockId))
                .ReturnsAsync(stock);

            // Act
            var result = await _controller.Delete(stockId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mockStockRepo.Verify(repo => repo.DeleteAsync(stockId), Times.Once);
        }
    }
}

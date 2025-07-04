using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Stocks.API.Dtos.Stock;
using Stocks.API.Helpers;
using Stocks.API.Models;
using Test.Mocks;
using Xunit;

namespace Test.Repository
{
    public class StockRepositoryTests
    {
        
        [Fact]
        public async Task GetAllAsync_ReturnsAllStocks_WhenNoQueryFiltersApplied()
        {
            // Arrange
            var repository = new MockStockRepository();
            var query = new QueryObject
            {
                PageNumber = 1,
                PageSize = 10
            };
            
            // Act
            var result = await repository.GetAllAsync(query);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Should().Contain(s => s.Symbol == "AAPL");
            result.Should().Contain(s => s.Symbol == "MSFT");
            result.Should().Contain(s => s.Symbol == "GOOGL");
        }
        
        [Fact]
        public async Task GetAllAsync_ReturnsFilteredStocks_WhenCompanyNameFilterApplied()
        {
            // Arrange
            var repository = new MockStockRepository();
            var query = new QueryObject
            {
                CompanyName = "Apple",
                PageNumber = 1,
                PageSize = 10
            };
            
            // Act
            var result = await repository.GetAllAsync(query);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Symbol.Should().Be("AAPL");
        }
        
        [Fact]
        public async Task GetAllAsync_ReturnsFilteredStocks_WhenSymbolFilterApplied()
        {
            // Arrange
            var repository = new MockStockRepository();
            var query = new QueryObject
            {
                Symbol = "MS",
                PageNumber = 1,
                PageSize = 10
            };
            
            // Act
            var result = await repository.GetAllAsync(query);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Symbol.Should().Be("MSFT");
        }
        
        [Fact]
        public async Task GetAllAsync_ReturnsSortedStocks_WhenSortbySymbolAscending()
        {
            // Arrange
            var repository = new MockStockRepository();
            var query = new QueryObject
            {
                Sortby = "Symbol",
                IsDescending = false,
                PageNumber = 1,
                PageSize = 10
            };
            
            // Act
            var result = await repository.GetAllAsync(query);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result[0].Symbol.Should().Be("AAPL");
            result[1].Symbol.Should().Be("GOOGL");
            result[2].Symbol.Should().Be("MSFT");
        }
        
        [Fact]
        public async Task GetAllAsync_ReturnsSortedStocks_WhenSortbySymbolDescending()
        {
            // Arrange
            var repository = new MockStockRepository();
            var query = new QueryObject
            {
                Sortby = "Symbol",
                IsDescending = true,
                PageNumber = 1,
                PageSize = 10
            };
            
            // Act
            var result = await repository.GetAllAsync(query);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result[0].Symbol.Should().Be("MSFT");
            result[1].Symbol.Should().Be("GOOGL");
            result[2].Symbol.Should().Be("AAPL");
        }
        
        [Fact]
        public async Task GetAllAsync_ReturnsPaginatedResults_WhenPageSizeIsLimited()
        {
            // Arrange
            var repository = new MockStockRepository();
            var query = new QueryObject
            {
                PageNumber = 1,
                PageSize = 2
            };
            
            // Act
            var result = await repository.GetAllAsync(query);
            
            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }
        
        [Fact]
        public async Task GetByIdAsync_ReturnsStock_WhenStockExists()
        {
            // Arrange
            var repository = new MockStockRepository();
            var stockId = 1;
            
            // Act
            var result = await repository.GetByIdAsync(stockId);
            
            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(stockId);
            result.Symbol.Should().Be("AAPL");
        }
        
        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenStockDoesNotExist()
        {
            // Arrange
            var repository = new MockStockRepository();
            var stockId = 999;
            
            // Act
            var result = await repository.GetByIdAsync(stockId);
            
            // Assert
            result.Should().BeNull();
        }
        
        [Fact]
        public async Task CreateAsync_AddsNewStock_AndReturnsIt()
        {
            // Arrange
            var repository = new MockStockRepository();
            var newStock = new Stock
            {
                Symbol = "AMZN",
                CompanyName = "Amazon.com Inc.",
                Purchase = 3200.00m,
                LastDiv = 0.0m,
                Industry = "Technology",
                MarketCap = 1600000000000
            };
            
            // Act
            var result = await repository.CreateAsync(newStock);
            
            // Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.Symbol.Should().Be("AMZN");
            
            // Verify it exists in the repository
            var stock = await repository.GetByIdAsync(result.Id);
            stock.Should().NotBeNull();
            stock!.Symbol.Should().Be("AMZN");
        }
        
        [Fact]
        public async Task UpdateAsync_UpdatesExistingStock_AndReturnsIt()
        {
            // Arrange
            var repository = new MockStockRepository();
            var stockId = 1;
            var updateDto = new UpdateStockRequestDto
            {
                Symbol = "AAPL",
                CompanyName = "Apple Inc. Updated",
                Purchase = 160.00m,
                LastDiv = 0.6m,
                Industry = "Tech",
                MarketCap = 2100000000000
            };
            
            // Act
            var result = await repository.UpdateAsync(stockId, updateDto);
            
            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(stockId);
            result.CompanyName.Should().Be("Apple Inc. Updated");
            result.Purchase.Should().Be(160.00m);
            
            // Verify it was updated in the repository
            var stock = await repository.GetByIdAsync(stockId);
            stock.Should().NotBeNull();
            stock!.CompanyName.Should().Be("Apple Inc. Updated");
        }
        
        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenStockDoesNotExist()
        {
            // Arrange
            var repository = new MockStockRepository();
            var stockId = 999;
            var updateDto = new UpdateStockRequestDto
            {
                Symbol = "FAKE",
                CompanyName = "Fake Company",
                Purchase = 100.00m,
                LastDiv = 0.0m,
                Industry = "Tech",
                MarketCap = 1000000000
            };
            
            // Act
            var result = await repository.UpdateAsync(stockId, updateDto);
            
            // Assert
            result.Should().BeNull();
        }
        
        [Fact]
        public async Task DeleteAsync_RemovesStock_AndReturnsIt()
        {
            // Arrange
            var repository = new MockStockRepository();
            var stockId = 2;
            
            // Act
            var result = await repository.DeleteAsync(stockId);
            
            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(stockId);
            
            // Verify it was removed from the repository
            var stock = await repository.GetByIdAsync(stockId);
            stock.Should().BeNull();
        }
        
        [Fact]
        public async Task DeleteAsync_ReturnsNull_WhenStockDoesNotExist()
        {
            // Arrange
            var repository = new MockStockRepository();
            var stockId = 999;
            
            // Act
            var result = await repository.DeleteAsync(stockId);
            
            // Assert
            result.Should().BeNull();
        }
        
        [Fact]
        public async Task StockExists_ReturnsTrue_WhenStockExists()
        {
            // Arrange
            var repository = new MockStockRepository();
            var stockId = 1;
            
            // Act
            var result = await repository.StockExists(stockId);
            
            // Assert
            result.Should().BeTrue();
        }
        
        [Fact]
        public async Task StockExists_ReturnsFalse_WhenStockDoesNotExist()
        {
            // Arrange
            var repository = new MockStockRepository();
            var stockId = 999;
            
            // Act
            var result = await repository.StockExists(stockId);
            
            // Assert
            result.Should().BeFalse();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stocks.API.Dtos.Stock;
using Stocks.API.Helpers;
using Stocks.API.Interfaces;
using Stocks.API.Models;

namespace Test.Mocks
{
    public class MockStockRepository : IStockRepository
    {
        private readonly List<Stock> _stocks;

        public MockStockRepository()
        {
            // Initialize with some test data
            _stocks = new List<Stock>
            {
                new Stock
                {
                    Id = 1,
                    Symbol = "AAPL",
                    CompanyName = "Apple Inc.",
                    Purchase = 150.00m,
                    LastDiv = 0.5m,
                    Industry = "Technology",
                    MarketCap = 2000000000000
                },
                new Stock
                {
                    Id = 2,
                    Symbol = "MSFT",
                    CompanyName = "Microsoft Corporation",
                    Purchase = 250.00m,
                    LastDiv = 0.6m,
                    Industry = "Technology",
                    MarketCap = 1800000000000
                },
                new Stock
                {
                    Id = 3,
                    Symbol = "GOOGL",
                    CompanyName = "Alphabet Inc.",
                    Purchase = 2800.00m,
                    LastDiv = 0.0m,
                    Industry = "Technology",
                    MarketCap = 1500000000000
                }
            };
        }

        public Task<Stock> CreateAsync(Stock stockModel)
        {
            stockModel.Id = _stocks.Max(s => s.Id) + 1;
            _stocks.Add(stockModel);
            return Task.FromResult(stockModel);
        }

        public Task<Stock?> DeleteAsync(int id)
        {
            var stock = _stocks.FirstOrDefault(s => s.Id == id);
            if (stock != null)
            {
                _stocks.Remove(stock);
            }
            return Task.FromResult(stock);
        }

        public Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = _stocks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(query.Sortby))
            {
                if (query.Sortby.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
                else if (query.Sortby.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.CompanyName) : stocks.OrderBy(s => s.CompanyName);
                }
                else if (query.Sortby.Equals("Industry", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Industry) : stocks.OrderBy(s => s.Industry);
                }
                else if (query.Sortby.Equals("Purchase", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Purchase) : stocks.OrderBy(s => s.Purchase);
                }
                else if (query.Sortby.Equals("LastDiv", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.LastDiv) : stocks.OrderBy(s => s.LastDiv);
                }
                else if (query.Sortby.Equals("MarketCap", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.MarketCap) : stocks.OrderBy(s => s.MarketCap);
                }
            }

            // Apply pagination
            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            return Task.FromResult(stocks.Skip(skipNumber).Take(query.PageSize).ToList());
        }

        public Task<Stock?> GetByIdAsync(int id)
        {
            var stock = _stocks.FirstOrDefault(s => s.Id == id);
            return Task.FromResult(stock);
        }

        public Task<bool> StockExists(int id)
        {
            return Task.FromResult(_stocks.Any(s => s.Id == id));
        }

        public Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var stock = _stocks.FirstOrDefault(s => s.Id == id);
            if (stock == null)
            {
                return Task.FromResult<Stock?>(null);
            }

            stock.Symbol = stockDto.Symbol;
            stock.CompanyName = stockDto.CompanyName;
            stock.Purchase = stockDto.Purchase;
            stock.LastDiv = stockDto.LastDiv;
            stock.Industry = stockDto.Industry;
            stock.MarketCap = stockDto.MarketCap;

            return Task.FromResult<Stock?>(stock);
        }
    }
}

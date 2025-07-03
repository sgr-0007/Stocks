using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stocks.API.Dtos.Stock;
using Stocks.API.Helpers;
using Stocks.API.Models;

namespace Stocks.API.Interfaces
{
    public interface IStockRepository
    {
        public Task<List<Stock>> GetAllAsync(QueryObject query);
        public Task<Stock?> GetByIdAsync(int id);
        public Task<Stock> CreateAsync(Stock stockModel);
        public Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto);
        public Task<Stock?> DeleteAsync(int id);
        public Task<bool> StockExists(int id);

    }
}
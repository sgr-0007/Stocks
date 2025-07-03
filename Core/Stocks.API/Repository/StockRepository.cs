using Stocks.API.Data;
using Stocks.API.Dtos.Stock;
using Stocks.API.Helpers;
using Stocks.API.Interfaces;
using Stocks.API.Mappers;
using Stocks.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Stocks.API.Repository
{
    public class StockRepository(ApplicationDBContext context) : IStockRepository
    {
        private readonly ApplicationDBContext _context = context;

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel is null) return null;
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;


        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = _context.Stocks.Include(x => x.Comments).AsQueryable();

            if (query.CompanyName is { Length: > 0 })
            {
                stocks = stocks.Where(x => x.CompanyName.Contains(query.CompanyName));
            }

            if (query.Symbol is { Length: > 0 })
            {
                stocks = stocks.Where(x => x.Symbol.Contains(query.Symbol));
            }
            if (query.Sortby is { Length: > 0 })
            {
                if (query.Sortby.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(x => x.Symbol) : stocks.OrderBy(x => x.Symbol);
                }
                if (query.Sortby.Equals("CompanyName", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(x => x.CompanyName) : stocks.OrderBy(x => x.CompanyName);
                }
            }
            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> StockExists(int id)
        {
            return await _context.Stocks.AnyAsync(x => x.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingStock is null) return null;
            stockDto.MapStockDtoToStockModel(existingStock);
            await _context.SaveChangesAsync();
            return existingStock;

        }
    }
}
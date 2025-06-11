using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNET8.Dtos.Stock;
using dotNET8.Models;

namespace dotNET8.Interfaces
{
    public interface IStockRepository
    {
         public Task<List<Stock>> GetAllAsync();
         public Task<Stock?> GetByIdAsync(int id);
         public Task<Stock> CreateAsync(Stock stockModel);
         public Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockModel);
         public Task<Stock?> DeleteAsync(int id);

    }
}
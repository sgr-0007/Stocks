using Stocks.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Stocks.API.Data
{
    public class ApplicationDBContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    {
        public required DbSet<Stock> Stocks { get; set; }
        public required DbSet<Comment> Comments { get; set; }
    }
}
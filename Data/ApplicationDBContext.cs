using dotNET8.Models;
using Microsoft.EntityFrameworkCore;

namespace dotNET8.Data
{
    public class ApplicationDBContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
    {
        public required DbSet<Stock> Stocks { get; set; }
        public required DbSet<Comment> Comments { get; set; }
    }
}
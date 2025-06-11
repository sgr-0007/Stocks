using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNET8.Models;

namespace dotNET8.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id);
    }
}
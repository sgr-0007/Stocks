using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNET8.Data;
using dotNET8.Interfaces;
using dotNET8.Models;
using Microsoft.EntityFrameworkCore;

namespace dotNET8.Repository
{
    public class CommentRepository(ApplicationDBContext context) : ICommentRepository
    {
        private readonly ApplicationDBContext _context = context;
        public async Task<List<Comment>> GetAllAsync()
        {
           return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            
        }
    }
}
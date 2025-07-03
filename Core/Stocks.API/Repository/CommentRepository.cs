using System.Xml.Serialization;
using Stocks.API.Data;
using Stocks.API.Dtos.Comment;
using Stocks.API.Interfaces;
using Stocks.API.Mappers;
using Stocks.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Stocks.API.Repository
{
    public class CommentRepository(ApplicationDBContext context) : ICommentRepository
    {
        private readonly ApplicationDBContext _context = context;

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;

        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (commentModel is null) return null;
            _context.Comments.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;

        }

        public async Task<List<Comment>> GetAllAsync()
        {
           return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            
        }

        public async Task<Comment?> UpdateAsync(int id, UpdateCommentRequestDto commentDto)
        {
            var existingComment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (existingComment is null) return null;
            commentDto.MapCommentDtoToComment(existingComment);
            await _context.SaveChangesAsync();
            return existingComment;
        }
    }
}
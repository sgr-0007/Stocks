using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNET8.Dtos.Comment;
using dotNET8.Models;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace dotNET8.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment commentModel);
        Task<Comment?> UpdateAsync(int id, UpdateCommentRequestDto commentDto);
        Task<Comment?> DeleteAsync(int id);
    }
}
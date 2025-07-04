using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stocks.API.Dtos.Comment;
using Stocks.API.Interfaces;
using Stocks.API.Models;

namespace Test.Mocks
{
    public class MockCommentRepository : ICommentRepository
    {
        private readonly List<Comment> _comments;

        public MockCommentRepository()
        {
            // Initialize with some test data
            _comments = new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    Title = "Great Investment",
                    Content = "This stock has been performing well",
                    StockId = 1
                },
                new Comment
                {
                    Id = 2,
                    Title = "Concerns",
                    Content = "I have some concerns about recent news",
                    StockId = 1
                },
                new Comment
                {
                    Id = 3,
                    Title = "Long-term outlook",
                    Content = "Looking good for the next 5 years",
                    StockId = 1
                }
            };
        }

        public Task<Comment> CreateAsync(Comment commentModel)
        {
            commentModel.Id = _comments.Max(c => c.Id) + 1;
            _comments.Add(commentModel);
            return Task.FromResult(commentModel);
        }

        public Task<Comment?> DeleteAsync(int id)
        {
            var comment = _comments.FirstOrDefault(c => c.Id == id);
            if (comment != null)
            {
                _comments.Remove(comment);
            }
            return Task.FromResult(comment);
        }

        public Task<List<Comment>> GetAllAsync()
        {
            return Task.FromResult(_comments.ToList());
        }

        public Task<Comment?> GetByIdAsync(int id)
        {
            var comment = _comments.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(comment);
        }

        public Task<Comment?> UpdateAsync(int id, UpdateCommentRequestDto commentDto)
        {
            var comment = _comments.FirstOrDefault(c => c.Id == id);
            if (comment == null)
            {
                return Task.FromResult<Comment?>(null);
            }

            comment.Title = commentDto.Title;
            comment.Content = commentDto.Content;

            return Task.FromResult<Comment?>(comment);
        }
    }
}

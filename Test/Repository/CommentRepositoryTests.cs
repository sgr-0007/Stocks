using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Stocks.API.Dtos.Comment;
using Stocks.API.Models;
using Test.Mocks;
using Xunit;

namespace Test.Repository
{
    public class CommentRepositoryTests
    {
        
        [Fact]
        public async Task GetAllAsync_ReturnsAllComments()
        {
            // Arrange
            var repository = new MockCommentRepository();
            
            // Act
            var result = await repository.GetAllAsync();
            
            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            result.Should().Contain(c => c.Title == "Great Investment");
            result.Should().Contain(c => c.Title == "Concerns");
            result.Should().Contain(c => c.Title == "Long-term outlook");
        }
        
        [Fact]
        public async Task GetByIdAsync_ReturnsComment_WhenCommentExists()
        {
            // Arrange
            var repository = new MockCommentRepository();
            var commentId = 1;
            
            // Act
            var result = await repository.GetByIdAsync(commentId);
            
            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(commentId);
            result.Title.Should().Be("Great Investment");
        }
        
        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenCommentDoesNotExist()
        {
            // Arrange
            var repository = new MockCommentRepository();
            var commentId = 999;
            
            // Act
            var result = await repository.GetByIdAsync(commentId);
            
            // Assert
            result.Should().BeNull();
        }
        
        [Fact]
        public async Task CreateAsync_AddsNewComment_AndReturnsIt()
        {
            // Arrange
            var repository = new MockCommentRepository();
            var newComment = new Comment
            {
                Title = "New Insight",
                Content = "Just discovered something interesting about this stock",
                StockId = 1
            };
            
            // Act
            var result = await repository.CreateAsync(newComment);
            
            // Assert
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.Title.Should().Be("New Insight");
            
            // Verify it was added to the repository
            var comment = await repository.GetByIdAsync(result.Id);
            comment.Should().NotBeNull();
            comment!.Title.Should().Be("New Insight");
        }
        
        [Fact]
        public async Task UpdateAsync_UpdatesExistingComment_AndReturnsIt()
        {
            // Arrange
            var repository = new MockCommentRepository();
            var commentId = 1;
            var updateDto = new UpdateCommentRequestDto
            {
                Title = "Updated Investment Opinion",
                Content = "I've changed my mind about this stock"
            };
            
            // Act
            var result = await repository.UpdateAsync(commentId, updateDto);
            
            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(commentId);
            result.Title.Should().Be("Updated Investment Opinion");
            result.Content.Should().Be("I've changed my mind about this stock");
            
            // Verify it was updated in the repository
            var comment = await repository.GetByIdAsync(commentId);
            comment.Should().NotBeNull();
            comment!.Title.Should().Be("Updated Investment Opinion");
        }
        
        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenCommentDoesNotExist()
        {
            // Arrange
            var repository = new MockCommentRepository();
            var commentId = 999;
            var updateDto = new UpdateCommentRequestDto
            {
                Title = "This Won't Work",
                Content = "Because the comment doesn't exist"
            };
            
            // Act
            var result = await repository.UpdateAsync(commentId, updateDto);
            
            // Assert
            result.Should().BeNull();
        }
        
        [Fact]
        public async Task DeleteAsync_RemovesComment_AndReturnsIt()
        {
            // Arrange
            var repository = new MockCommentRepository();
            var commentId = 2;
            
            // Act
            var result = await repository.DeleteAsync(commentId);
            
            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(commentId);
            
            // Verify it was removed from the repository
            var comment = await repository.GetByIdAsync(commentId);
            comment.Should().BeNull();
        }
        
        [Fact]
        public async Task DeleteAsync_ReturnsNull_WhenCommentDoesNotExist()
        {
            // Arrange
            var repository = new MockCommentRepository();
            var commentId = 999;
            
            // Act
            var result = await repository.DeleteAsync(commentId);
            
            // Assert
            result.Should().BeNull();
        }
    }
}

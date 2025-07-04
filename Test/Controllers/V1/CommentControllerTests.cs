using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Stocks.API.Controllers.V1;
using Stocks.API.Dtos.Comment;
using Stocks.API.Interfaces;
using Stocks.API.Models;
using Xunit;

namespace Test.Controllers.V1
{
    public class CommentControllerTests
    {
        private readonly Mock<ICommentRepository> _mockCommentRepo;
        private readonly Mock<IStockRepository> _mockStockRepo;
        private readonly CommentController _controller;

        public CommentControllerTests()
        {
            _mockCommentRepo = new Mock<ICommentRepository>();
            _mockStockRepo = new Mock<IStockRepository>();
            _controller = new CommentController(_mockCommentRepo.Object, _mockStockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfCommentDtos()
        {
            // Arrange
            var comments = new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    Title = "Great Stock",
                    Content = "This stock is performing well",
                    StockId = 1
                },
                new Comment
                {
                    Id = 2,
                    Title = "Concerns",
                    Content = "I have some concerns about this stock",
                    StockId = 1
                }
            };

            _mockCommentRepo.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(comments);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var commentDtos = okResult.Value.Should().BeAssignableTo<IEnumerable<CommentDto>>().Subject;
            commentDtos.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAll_WithInvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetById_WithValidId_ReturnsOkResult_WithCommentDto()
        {
            // Arrange
            int commentId = 1;
            var comment = new Comment
            {
                Id = commentId,
                Title = "Great Stock",
                Content = "This stock is performing well",
                StockId = 1
            };

            _mockCommentRepo.Setup(repo => repo.GetByIdAsync(commentId))
                .ReturnsAsync(comment);

            // Act
            var result = await _controller.GetById(commentId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var commentDto = okResult.Value.Should().BeAssignableTo<CommentDto>().Subject;
            commentDto.Id.Should().Be(commentId);
            commentDto.Title.Should().Be("Great Stock");
        }

        [Fact]
        public async Task GetById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            int commentId = 999;
            _mockCommentRepo.Setup(repo => repo.GetByIdAsync(commentId))
                .ReturnsAsync((Comment?)null);

            // Act
            var result = await _controller.GetById(commentId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetById_WithInvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await _controller.GetById(1);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Create_WithValidData_ReturnsCreatedAtAction_WithCommentDto()
        {
            // Arrange
            int stockId = 1;
            var createDto = new CreateCommentRequestDto
            {
                Title = "New Comment",
                Content = "This is a new comment"
            };

            var createdComment = new Comment
            {
                Id = 3,
                Title = "New Comment",
                Content = "This is a new comment",
                StockId = stockId
            };

            _mockStockRepo.Setup(repo => repo.StockExists(stockId))
                .ReturnsAsync(true);

            _mockCommentRepo.Setup(repo => repo.CreateAsync(It.IsAny<Comment>()))
                .Callback<Comment>(comment => comment.Id = 3)
                .ReturnsAsync(createdComment);

            // Act
            var result = await _controller.Create(stockId, createDto);

            // Assert
            var createdAtActionResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdAtActionResult.ActionName.Should().Be("GetById");
            createdAtActionResult.RouteValues.Should().ContainKey("id").And.ContainValue(3);
            var commentDto = createdAtActionResult.Value.Should().BeAssignableTo<CommentDto>().Subject;
            commentDto.Title.Should().Be("New Comment");
        }

        [Fact]
        public async Task Create_WithInvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await _controller.Create(1, new CreateCommentRequestDto());

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Create_WithNonExistentStock_ReturnsBadRequest()
        {
            // Arrange
            int stockId = 999;
            var createDto = new CreateCommentRequestDto
            {
                Title = "New Comment",
                Content = "This is a new comment"
            };

            _mockStockRepo.Setup(repo => repo.StockExists(stockId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Create(stockId, createDto);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.Value.Should().Be("Stock does not exist");
        }

        [Fact]
        public async Task Update_WithValidIdAndData_ReturnsOkResult_WithUpdatedCommentDto()
        {
            // Arrange
            int commentId = 1;
            var updateDto = new UpdateCommentRequestDto
            {
                Title = "Updated Comment",
                Content = "This comment has been updated"
            };

            var updatedComment = new Comment
            {
                Id = commentId,
                Title = "Updated Comment",
                Content = "This comment has been updated",
                StockId = 1
            };

            _mockCommentRepo.Setup(repo => repo.UpdateAsync(commentId, updateDto))
                .ReturnsAsync(updatedComment);

            // Act
            var result = await _controller.Update(commentId, updateDto);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var commentDto = okResult.Value.Should().BeAssignableTo<CommentDto>().Subject;
            commentDto.Id.Should().Be(commentId);
            commentDto.Title.Should().Be("Updated Comment");
        }

        [Fact]
        public async Task Update_WithInvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await _controller.Update(1, new UpdateCommentRequestDto());

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Update_WithInvalidId_ReturnsOkResultWithNull()
        {
            // Arrange
            int commentId = 999;
            var updateDto = new UpdateCommentRequestDto
            {
                Title = "Updated Comment",
                Content = "This comment has been updated"
            };

            _mockCommentRepo.Setup(repo => repo.UpdateAsync(commentId, updateDto))
                .ReturnsAsync((Comment?)null);

            // Act
            var result = await _controller.Update(commentId, updateDto);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeNull();
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsNoContent()
        {
            // Arrange
            int commentId = 1;
            var comment = new Comment { Id = commentId };

            _mockCommentRepo.Setup(repo => repo.DeleteAsync(commentId))
                .ReturnsAsync(comment);

            // Act
            var result = await _controller.Delete(commentId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mockCommentRepo.Verify(repo => repo.DeleteAsync(commentId), Times.Once);
        }

        [Fact]
        public async Task Delete_WithInvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await _controller.Delete(1);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}

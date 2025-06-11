using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using dotNET8.Dtos.Comment;
using dotNET8.Interfaces;
using dotNET8.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace dotNET8.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/comment")]
    [ApiController]
    public class CommentController(ICommentRepository commentRepo) : ControllerBase
    {
        private readonly ICommentRepository _commentRepo = commentRepo;

       [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();
            var commentDto = comments.Select(x => x.ToCommentDto());
            return Ok(commentDto);

        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);
            return comment is null ? NotFound() : Ok(comment.ToCommentDto());
        }

    }
}
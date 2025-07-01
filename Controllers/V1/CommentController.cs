using System;
using System.Collections.Generic;
using System.Data;
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
    [Route("api/v{version:apiVersion}/comments")]
    [ApiController]
    public class CommentController(ICommentRepository commentRepo, IStockRepository stockRepo) : ControllerBase
    {
        private readonly ICommentRepository _commentRepo = commentRepo;
        private readonly IStockRepository _stockRepo = stockRepo;


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comments = await _commentRepo.GetAllAsync();
            var commentDto = comments.Select(x => x.ToCommentDto());
            return Ok(commentDto);

        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepo.GetByIdAsync(id);
            return comment is null ? NotFound() : Ok(comment.ToCommentDto());
        }
        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentRequestDto commentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockExists = await _stockRepo.StockExists(stockId);
            if (!stockExists)
            {
                return BadRequest("Stock does not exist");
            }
            var commentModel = commentDto.ToComment(stockId);
            await _commentRepo.CreateAsync(commentModel);
            return CreatedAtAction("GetById", new { id = commentModel.Id }, commentModel.ToCommentDto());

        }
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto commentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var commentModel = await _commentRepo.UpdateAsync(id, commentDto);
            return Ok(commentModel?.ToCommentDto());

        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            await _commentRepo.DeleteAsync(id);
            return NoContent();

        }

    }
}
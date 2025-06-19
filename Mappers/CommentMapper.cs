using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNET8.Dtos.Comment;
using dotNET8.Models;

namespace dotNET8.Mappers
{
    public static class CommentMapper
    {

        public static CommentDto ToCommentDto(this Comment commentModel)
        {

            return new CommentDto
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                StockId = commentModel.StockId

            };
        }
        public static Comment ToComment(this CreateCommentRequestDto commentDto, int stockId)
        {

            return new Comment
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = stockId

            };
        }
        public static void MapCommentDtoToComment(this UpdateCommentRequestDto src, Comment dest)
        {
            dest.Title = src.Title;
            dest.Content = src.Content;
        }
    }
}
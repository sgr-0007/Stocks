
using System.ComponentModel.DataAnnotations;

namespace Stocks.API.Dtos.Comment
{
    public class CreateCommentRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title is five characters")]
        [MaxLength(280, ErrorMessage ="Title cannot be over 280 chars")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Content is five characters")]
        [MaxLength(280, ErrorMessage ="Content cannot be over 280 chars")]
        public string Content { get; set; } = string.Empty;

    }
}
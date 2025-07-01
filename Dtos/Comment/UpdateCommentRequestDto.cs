using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotNET8.Dtos.Comment
{
    public class UpdateCommentRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title is five characters")]
        [MaxLength(280, ErrorMessage ="Title cannot be over 280 chars")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Title is five characters")]
        [MaxLength(280, ErrorMessage ="Title cannot be over 280 chars")]
        public string Content { get; set; } = string.Empty;
    }
}
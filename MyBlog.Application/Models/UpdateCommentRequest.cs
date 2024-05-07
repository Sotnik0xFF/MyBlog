using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Models
{
    public record UpdateCommentRequest
    {
        [Required]
        public long Id { get; init; }

        [Required]
        public required string Title { get; init; }

        [Required]
        public required string Text { get; init; }
    }
}

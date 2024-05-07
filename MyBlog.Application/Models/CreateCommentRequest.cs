using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Models
{
    public record CreateCommentRequest
    {
        public CreateCommentRequest(long userId, long postId, string title, string text)
        {
            UserId = userId;
            PostId = postId;
            Title = title;
            Text = text;
        }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        public long PostId { get; set; }
    }
}

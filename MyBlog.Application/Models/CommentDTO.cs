using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Models
{
    public record CommentDTO
    {
        public CommentDTO(long id, long postId, long userId, string title, string text)
        {
            Id = id;
            PostId = postId;
            UserId = userId;
            Title = title;
            Text = text;
        }

        public long Id { get; }
        public long PostId { get; }
        public string Title { get; }
        public string Text { get; }
        public long UserId { get; }
    }
}

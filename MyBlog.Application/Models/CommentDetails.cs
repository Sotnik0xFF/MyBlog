using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Models
{
    public class CommentDetails
    {
        public long Id { get; init; }
        public long PostId { get; init; }
        public required string Title { get; init; }
        public required string Text { get; init; }
        public required string AuthorName { get; init; }
    }
}

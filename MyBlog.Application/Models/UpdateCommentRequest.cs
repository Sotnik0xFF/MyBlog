using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Models
{
    public class UpdateCommentRequest
    {
        public long Id { get; init; }
        public required string Title { get; init; }
        public required string Text { get; init; }
    }
}

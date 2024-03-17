using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Models
{
    public class CreateCommentRequest
    {
        public required string Title { get; set; }
        public required string Text { get; set; }
        public long UserId { get; set; }
        public long PostId { get; set; }
    }
}

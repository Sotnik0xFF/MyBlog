using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Models
{
    public class CreatePostRequest
    {
        public required string Title { get; set; }
        public required string Text { get; set; }
        public long AuthorId { get; set; }
        public required IEnumerable<string> Tags { get; set; }
    }
}

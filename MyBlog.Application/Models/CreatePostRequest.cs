using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Models
{
    public class CreatePostRequest
    {
        public string Title { get; set; } = String.Empty;
        public string Text { get; set; } = String.Empty;
        public long AuthorId { get; set; }
        public IEnumerable<string> TagNames { get; set; } = new List<String>();
    }
}

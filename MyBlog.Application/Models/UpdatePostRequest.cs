using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Models
{
    public class UpdatePostRequest
    {
        public long Id { get; set; } 
        public string Title { get; set; } = String.Empty;
        public string Text { get; set; } = String.Empty;
        public IEnumerable<string> TagNames { get; set; } = new List<string>();
    }
}

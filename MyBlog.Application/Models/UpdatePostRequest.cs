using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Models;

public class UpdatePostRequest
{
    public UpdatePostRequest(long postId, string title, string text, IEnumerable<string> tagNames)
    {
        PostId = postId;
        Title = title;
        Text = text;
        TagNames = tagNames;
    }

    public long PostId { get; } 
    public string Title { get; }
    public string Text { get; }
    public IEnumerable<string> TagNames { get; }
}

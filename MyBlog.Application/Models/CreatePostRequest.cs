using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Models;

public class CreatePostRequest
{
    public CreatePostRequest(long authorId, string title, string text, IEnumerable<string> tagNames)
    {
        Title = title;
        Text = text;
        TagNames = tagNames;
        AuthorId = authorId;
    }

    public string Title { get; }
    public string Text { get; }
    public long AuthorId { get; }
    public IEnumerable<string> TagNames { get; }
}

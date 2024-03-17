using MyBlog.Domain.Models;

namespace MyBlog.Application.Models;

public class PostDetails
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public required string Text { get; set; }
    public long AuthorId { get; set; }
    public List<string> Tags { get; set; } = new List<string>();
}

using MyBlog.Domain.Models;

namespace MyBlog.Application.Models;

public record PostDTO
{
    public PostDTO(long id, long authorId, string title, string text, IEnumerable<string> tags)
    {
        Id = id;
        AuthorId = authorId;
        Title = title;
        Text = text;
        Tags = tags;
    }

    public long Id { get; }
    public string Title { get; }
    public string Text { get; }
    public long  AuthorId { get; }
    public IEnumerable<string> Tags { get; }
}

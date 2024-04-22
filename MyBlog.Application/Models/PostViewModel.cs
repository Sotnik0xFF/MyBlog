using MyBlog.Domain.Models;

namespace MyBlog.Application.Models;

public record PostViewModel
{
    public long Id { get; init; }
    public required string Title { get; init; }
    public required string Text { get; init; }
    public required UserDTO Author { get; init; }
    public required IEnumerable<TagViewModel> Tags { get; init; }
    public required IEnumerable<CommentViewModel> Comments { get; init; }
}

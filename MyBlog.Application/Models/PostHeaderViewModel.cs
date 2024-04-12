namespace MyBlog.Application.Models;

public record PostHeaderViewModel
{
    public long Id { get; init; }
    public required string Title { get; init; }
    public required UserViewModel Author { get; init; }
    public required IEnumerable<TagViewModel> Tags { get; init; }

}

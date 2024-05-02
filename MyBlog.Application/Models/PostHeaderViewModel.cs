namespace MyBlog.Application.Models;

public record PostHeaderViewModel
{
    public long Id { get; init; }
    public required string Title { get; init; }
    public required UserDTO Author { get; init; }
    public required IEnumerable<TagDTO> Tags { get; init; }

}

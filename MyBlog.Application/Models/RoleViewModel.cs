namespace MyBlog.Application.Models;


public record RoleViewModel
{
    public RoleViewModel(long id, string name)
    {
        Id = id;
        Name = name;
    }

    public long Id { get; init; }
    public string Name { get; init; }
}

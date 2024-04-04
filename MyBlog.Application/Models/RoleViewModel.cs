namespace MyBlog.Application.Models;


public record RoleViewModel
{
    public RoleViewModel(long id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public long Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
}

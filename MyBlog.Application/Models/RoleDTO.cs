namespace MyBlog.Application.Models;


public record RoleDTO
{
    public RoleDTO(long id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public long Id { get; }
    public string Name { get; }
    public string Description { get; }
}

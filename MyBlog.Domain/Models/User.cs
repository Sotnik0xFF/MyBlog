using MyBlog.Domain.Base;

namespace MyBlog.Domain.Models;

public class User : Entity
{
    private readonly List<Role> _roles;
    private readonly string _login;
    private string _password;

    public User(string login, string password, string firstName, string lastName, string email)
    {
        _login = login;
        _password = password;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        _roles = new List<Role>();
    }

    public User(long id, string login, string password, string firstName, string lastName, string email) :
        this(login, password, firstName, lastName, email)
    {
        Id = id;
    }


    public string Login => _login;
    public string Password => _password;
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public IEnumerable<Role> Roles => _roles;

    public void AddRole(Role role) => _roles.Add(role);
    public void RemoveRole(Role role) => _roles.Remove(role);
}

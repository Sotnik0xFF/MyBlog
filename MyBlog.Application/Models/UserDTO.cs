using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Models;

public record UserDTO
{
    public UserDTO(long id, string firstName, string lastName, string email, IEnumerable<RoleDTO> roles)
    {
        Id= id;
        FirstName= firstName;
        LastName= lastName;
        Email= email;
        Roles= roles;
    }

    public long Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public IEnumerable<RoleDTO> Roles { get; }
}

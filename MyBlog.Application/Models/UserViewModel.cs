using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Models;

public record UserViewModel
{
    public long Id { get; init; }
    public required string Login { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required IEnumerable<RoleViewModel> Roles { get; init; }
}

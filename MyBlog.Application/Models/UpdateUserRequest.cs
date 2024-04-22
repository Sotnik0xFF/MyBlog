using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Models
{
    public class UpdateUserRequest
    {
        public UpdateUserRequest(long id, string firstName, string lastName, string? newPassword, IEnumerable<RoleDTO> roles)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            NewPassword = newPassword;
            Roles = roles;
        }

        public long Id { get; }
        public string FirstName { get; }
        public string LastName { get; }

        public string? NewPassword { get; }

        public IEnumerable<RoleDTO> Roles { get; }
    }
}

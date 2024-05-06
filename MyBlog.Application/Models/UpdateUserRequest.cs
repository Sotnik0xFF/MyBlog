using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Models
{
    public class UpdateUserRequest
    {
        public UpdateUserRequest(long id, string firstName, string lastName, string? newPassword, IEnumerable<long> rolesId)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            NewPassword = newPassword;
            RolesId = rolesId;
        }

        public long Id { get; }
        public string FirstName { get; }
        public string LastName { get; }

        public string? NewPassword { get; }

        public IEnumerable<long> RolesId { get; }
    }
}

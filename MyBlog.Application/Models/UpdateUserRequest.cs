using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Models
{
    public class UpdateUserRequest
    {
        public long Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public string? NewPassword { get; set; }

        public required IEnumerable<RoleDTO> Roles { get; set; }
    }
}

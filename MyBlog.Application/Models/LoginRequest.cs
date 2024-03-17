using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Models
{
    public class LoginRequest
    {
        public required string UserLogin { get; set; }
        public required string Password { get; set; }
    }
}

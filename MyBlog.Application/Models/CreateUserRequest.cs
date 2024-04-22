using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Models;

public class CreateUserRequest
{
    public CreateUserRequest(string firstName, string lastName, string password, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Password = password;
        Email = email;
    }

    public string Password { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
}

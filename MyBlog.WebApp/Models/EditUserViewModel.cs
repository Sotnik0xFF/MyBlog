using MyBlog.Application.Models;

namespace MyBlog.WebApp.Models
{
    public class EditUserViewModel
    {
        public long Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? NewPassword { get; set; }
        public required IEnumerable<string> UserRoleNames { get; set; }
        public required IEnumerable<string> AllRoleNames { get; set; }
    }
}

using MyBlog.Application.Models;

namespace MyBlog.WebApp.Models
{
    public class UserDetailsViewModel
    {
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public IEnumerable<string> RoleNames { get; set; } = new List<string>();
    }
}

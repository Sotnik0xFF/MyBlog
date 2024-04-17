using MyBlog.Application.Models;

namespace MyBlog.WebApp.Models
{
    public class EditUserViewModel
    {
        public UpdateUserRequest Request { get; set; }
        public long Id { get; set; }
        public IEnumerable<RoleViewModel> AllRoles { get; set; }
    }
}

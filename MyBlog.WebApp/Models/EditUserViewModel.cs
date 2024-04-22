using MyBlog.Application.Models;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.WebApp.Models
{
    public class EditUserViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Укажите имя.")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Укажите фамилию.")]
        public required string LastName { get; set; }

        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Добавьте хотя бы одну роль.")]
        public required IEnumerable<string> UserRoleNames { get; set; }
        public IEnumerable<string> AllRoleNames { get; set; } = new List<string>();
    }
}

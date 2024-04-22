using System.ComponentModel.DataAnnotations;

namespace MyBlog.WebApp.Models
{
    public class AccountRegisterViewModel
    {
        [Required(ErrorMessage = "Пароль не может быть пустым.")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Укажите имя.")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Укажите фамилию.")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Укажите E-Mail.")]
        public required string Email { get; set; }
    }
}

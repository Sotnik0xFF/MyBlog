using System.ComponentModel.DataAnnotations;

namespace MyBlog.WebApp.Models
{
    public class NewCommentViewModel
    {
        [Required(ErrorMessage = "Заголовок комментария не должен быть пустым.")]
        public string Title { get; } = String.Empty;
        [Required(ErrorMessage = "Заголовок комментария не должен быть пустым.")]
        public string Text { get; } = String.Empty;
        public long PostId { get; }
    }
}

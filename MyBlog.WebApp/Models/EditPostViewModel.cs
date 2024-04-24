using MyBlog.Application.Models;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.WebApp.Models;

public class EditPostViewModel
{
    public long Id { get; set; }
    [Required(ErrorMessage = "Заголовок не может быть пустым")]
    public required string Title { get; set; }
    [Required(ErrorMessage = "Текст статьи не может быть пустым")]
    public required string Text { get; set; }
    public IEnumerable<string> PostTagNames { get; set; } = new List<string>();
    public IEnumerable<string> AllTagNames { get; set; } = new List<string>();
}

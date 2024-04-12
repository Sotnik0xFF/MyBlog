using MyBlog.Application.Models;

namespace MyBlog.WebApp.Models;

public class EditPostViewModel
{
    public required IEnumerable<TagViewModel> AllTags { get; set; }
    public UpdatePostRequest Request { get; set; } = new ();
}

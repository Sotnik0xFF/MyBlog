using MyBlog.Application.Models;

namespace MyBlog.WebApp.Models
{
    public class CreatePostViewModel
    {
        public required IEnumerable<TagViewModel> AllTags {get; set;}
        public CreatePostRequest Request { get; set; } = new CreatePostRequest();
    }
}

using MyBlog.Application.Models;

namespace MyBlog.WebApp.Models
{
    public class PostDetailsViewModel
    {
        public long Id { get; set; }
        public IEnumerable<string> Tags { get; set; } = new List<string>();
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public long AuthorId { get; set; }
        public string AuthorFirstName { get; set; } = string.Empty;
        public string AuthorLastName { get; set; } = string.Empty;
        public string commentTitle { get; set; } = string.Empty;
        public string commentText { get; set; } = string.Empty;
        public IEnumerable<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();
    }
}

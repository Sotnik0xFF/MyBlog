namespace MyBlog.WebApp.Models
{
    public class CommentViewModel
    {
        public long Id { get; set; }
        public string UserFirstName { get; set; } = string.Empty;
        public string UserLastName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;

    }
}

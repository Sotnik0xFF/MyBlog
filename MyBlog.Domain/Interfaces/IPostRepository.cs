using MyBlog.Domain.Base;
using MyBlog.Domain.Models;

namespace MyBlog.Domain.Interfaces
{
    public interface IPostRepository : IRepository
    {
        void Add(Post post);
        void Update(Post post);
        void Delete(Post post);
        Task<Post?> FindById(long id);
        Task<IEnumerable<Post>> FindByUserId(long userId);
        Task<IEnumerable<Post>> FindAll();
    }
}

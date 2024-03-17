using MyBlog.Domain.Base;
using MyBlog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Domain.Interfaces
{
    public interface ICommentRepository : IRepository
    {
        void Add(Comment comment);
        void Update(Comment comment);
        void Delete(Comment comment);
        Task<Comment?> FindById(long id);
        Task<IEnumerable<Comment>> FindByPostId(long postId);
        Task<IEnumerable<Comment>> FindByUserId(long postId);
        Task<IEnumerable<Comment>> FindAll();
    }
}

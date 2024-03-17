using Microsoft.EntityFrameworkCore;
using MyBlog.Domain.Base;
using MyBlog.Domain.Interfaces;
using MyBlog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly MyBlogDBContext _context;

        public CommentRepository(MyBlogDBContext context)
        {
            _context = context;
        }

        public void Add(Comment comment)
        {
            _context.Comments.Add(comment);
        }

        public void Delete(Comment comment)
        {
            _context.Remove(comment);
        }

        public async Task<IEnumerable<Comment>> FindAll()
        {
            return await _context.Comments.ToArrayAsync();
        }

        public async Task<Comment?> FindById(long id)
        {
            return await _context.Comments.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Comment>> FindByPostId(long postId)
        {
            return await _context.Comments.Where(e => e.PostId == postId).ToArrayAsync();
        }

        public async Task<IEnumerable<Comment>> FindByUserId(long userId)
        {
            return await _context.Comments.Where(e => e.UserId == userId).ToArrayAsync();
        }

        public void Update(Comment comment)
        {
            _context.Comments.Update(comment);
        }

        public IUnitOfWork UnitOfWork => _context;
    }
}

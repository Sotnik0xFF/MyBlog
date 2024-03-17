using Microsoft.EntityFrameworkCore;
using MyBlog.Domain.Base;
using MyBlog.Domain.Interfaces;
using MyBlog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private readonly MyBlogDBContext _context;

    public PostRepository(MyBlogDBContext context)
    {
        _context = context;
    }

    public void Add(Post post)
    {
        _context.Posts.Add(post);
    }

    public void Update(Post post)
    {
        _context.Update(post);
    }

    public void Delete(Post post)
    {
        _context.Remove(post);
    }

    public async Task<Post?> FindById(long id)
    {
        Post? post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == id);
        if (post != null)
        {
            await LoadTags(post);
        }
        return post;
    }

    public async Task<IEnumerable<Post>> FindAll()
    {
        IEnumerable<Post> posts = await _context.Posts.ToListAsync();
        foreach (Post post in posts)
        {
           await LoadTags(post);
        }
        return posts;
    }

    public async Task<IEnumerable<Post>> FindByUserId(long userId)
    {
        IEnumerable<Post> posts = await _context.Posts.Where(p => p.UserId == userId).ToListAsync();
        foreach (var post in posts)
        {
            await LoadTags(post);
        }
        return posts;
    }

    public IUnitOfWork UnitOfWork => _context;

    private async Task LoadTags(Post post)
    {
        await _context.Entry(post).Collection(p => p.Tags).LoadAsync();
    }
}

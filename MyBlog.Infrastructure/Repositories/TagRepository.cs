using Microsoft.EntityFrameworkCore;
using MyBlog.Domain.Base;
using MyBlog.Domain.Interfaces;
using MyBlog.Domain.Models;

namespace MyBlog.Infrastructure.Repositories;

public class TagRepository : ITagRepository
{
    private readonly MyBlogDBContext _context;

    public TagRepository(MyBlogDBContext context)
    {
        _context = context;
    }

    public void Add(Tag tag)
    {
        _context.Tags.Add(tag);
    }

    public void Update(Tag tag)
    {
        _context.Tags.Update(tag);
    }

    public void Delete(Tag tag)
    {
        _context.Tags.Remove(tag);
    }

    public async Task<IEnumerable<Tag>> FindAll()
    {
        IEnumerable<Tag> tags = await _context.Tags.ToListAsync();
        return tags;
    }

    public async Task<Tag?> FindById(long id)
    {
        Tag? tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
        return tag;
    }

    public async Task<Tag?> FindByValue(string value)
    {
        Tag? tag = await _context.Tags.FirstOrDefaultAsync(t => t.Value == value);
        return tag;
    }

    public IUnitOfWork UnitOfWork => _context;
}

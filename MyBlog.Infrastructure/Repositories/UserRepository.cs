using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyBlog.Domain.Base;
using MyBlog.Domain.Interfaces;
using MyBlog.Domain.Models;

namespace MyBlog.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MyBlogDBContext _context;

    public UserRepository(MyBlogDBContext context)
    {
        _context = context;
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    public void Delete(User user)
    {
        _context.Users.Remove(user);
    }

    public async Task<User?> FindById(long id)
    {
        User? user = await _context.Users.FindAsync(id);
        LoadRoles(user);
        return user;
    }

		public async Task<User?> FindByEmail(string email)
		{
			User? user = await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
			LoadRoles(user);
			return user;
		}

		public async Task<IEnumerable<User>> FindAll()
    {
        List<User> users = await _context.Users.ToListAsync();
        foreach (User user in users)
        {
				LoadRoles(user);
			}
        return users;
    }

		public IUnitOfWork UnitOfWork => _context;

    private void LoadRoles(User? user)
    {
			if (user != null)
			{
				_context.Entry(user).Collection(u => u.Roles).Load();
			}
		}
}

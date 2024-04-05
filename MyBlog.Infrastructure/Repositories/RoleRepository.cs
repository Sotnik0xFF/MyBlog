using Microsoft.EntityFrameworkCore;
using MyBlog.Domain.Interfaces;
using MyBlog.Domain.Models;

namespace MyBlog.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private const long UserRoleId = 1L;
    private const long ModeratorRoleId = 2L;
    private const long AdministratorRoleId = 3L;

    private readonly MyBlogDBContext _context;

    public RoleRepository(MyBlogDBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Role>> FindAll()
    {
        return await _context.Roles.ToListAsync();
    }

    public async Task<Role> GetAdministratorRole()
    {
        Role role = await _context.Roles.FirstAsync(r => r.Id == AdministratorRoleId);
        return role;
    }

    public async Task<Role> GetModeratorRole()
    {
        Role role = await _context.Roles.FirstAsync(r => r.Id == ModeratorRoleId);
        return role;
    }

    public async Task<Role> GetUserRole()
    {
        Role role = await _context.Roles.FirstAsync(r => r.Id == UserRoleId);
        return role;
    }
}

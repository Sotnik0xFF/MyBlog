using MyBlog.Domain.Models;

namespace MyBlog.Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role> GetAdministratorRole();
        Task<Role> GetModeratorRole();
        Task<Role> GetUserRole();

        Task<IEnumerable<Role>> FindAll();
    }
}

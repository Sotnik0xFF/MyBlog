using MyBlog.Domain.Base;
using MyBlog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Domain.Interfaces
{
    public interface IUserRepository : IRepository
    {
        void Add(User user);
        void Update(User user);
        void Delete(User user);
        Task<User?> FindById(long id);
		Task<User?> FindByLogin(string login);
		Task<IEnumerable<User>> FindAll();
    }
}

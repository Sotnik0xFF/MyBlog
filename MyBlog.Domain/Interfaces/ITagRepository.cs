using MyBlog.Domain.Base;
using MyBlog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Domain.Interfaces
{
    public interface ITagRepository : IRepository
    {
        void Add(Tag tag);
        void Delete(Tag tag);
        void Update(Tag tag);
        Task<Tag?> FindById(long id);
        Task<Tag?> FindByValue(string value);
        Task<IEnumerable<Tag>> FindAll();
    }
}

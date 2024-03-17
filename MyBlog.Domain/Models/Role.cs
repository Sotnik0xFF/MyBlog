using MyBlog.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Domain.Models
{
    public class Role : Entity
    {
        public Role(string name)
        {
            Name = name;
        }

        public Role(long id, string name) : this(name)
        {
            Id = id;
        }
        public string Name { get; set; }
    }
}

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
        public Role(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public Role(long id, string name, string description) : this(name, description)
        {
            Id = id;
        }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

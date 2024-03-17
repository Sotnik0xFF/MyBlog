using MyBlog.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Domain.Models
{
    public class Tag : Entity
    {
        public Tag(string value)
        {
            Value = value;
        }

        public Tag(long id, string value) : this(value)
        {
            Id = id;
        }

        public string Value { get; set; }
    }
}

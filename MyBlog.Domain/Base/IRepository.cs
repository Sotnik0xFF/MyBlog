using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Domain.Base;

public interface IRepository
{
    IUnitOfWork UnitOfWork { get; }
}

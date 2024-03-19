using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Models;

public record TagViewModel
{
    public long Id { get; init; }
    public required string Name { get; init; }
}

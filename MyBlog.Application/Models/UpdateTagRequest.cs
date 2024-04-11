using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Application.Models;

public class UpdateTagRequest
{
    public long Id { get; set; }
    public required string NewTagName { get; set; }
}

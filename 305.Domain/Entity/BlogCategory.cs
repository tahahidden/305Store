using _305.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace _305.Domain.Entity;
public class BlogCategory : BaseEntity
{
    public string? description { get; set; }

    public ICollection<Blog> blogs { get; set; }
}

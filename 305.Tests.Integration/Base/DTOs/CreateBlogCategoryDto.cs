using System;
using System.Collections.Generic;
using System.Text;

namespace _305.Tests.Integration.Base.DTOs;
public class CreateBlogCategoryDto
{
    public string name { get; set; } = null!;
    public string slug { get; set; } = null!;
}

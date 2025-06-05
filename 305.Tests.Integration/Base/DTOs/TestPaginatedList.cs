using System;
using System.Collections.Generic;
using System.Text;

namespace _305.Tests.Integration.Base.DTOs;
public class TestPaginatedList<T>
{
    public List<T> Data { get; set; } = new();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
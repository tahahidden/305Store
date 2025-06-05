using System;
using System.Collections.Generic;
using System.Text;

namespace _305.Tests.Integration.Base.DTOs;
public class TestResponseDto<T>
{
    public bool is_success { get; set; }
    public int response_code { get; set; }
    public string? message { get; set; }
    public T? data { get; set; }
}

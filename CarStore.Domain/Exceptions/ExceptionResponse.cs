using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Exceptions
{
    public class ExceptionResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "An unexpected error occurred";
        public object? Errors { get; set; } = null;
    }
}

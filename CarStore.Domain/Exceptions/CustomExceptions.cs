using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Exceptions
{
    public class CustomExceptions
    {
        public class NotFoundException : Exception
        {
            public NotFoundException(string message) : base(message) { }
        }

        public class BadRequestException : Exception
        {
            public BadRequestException(string message) : base(message) { }
        }

        public class UnauthorizedException : Exception
        {
            public UnauthorizedException(string message) : base(message) { }
        }

        public class InternalServerException : Exception
        {
            public InternalServerException(string message) : base(message) { }
        }

        public class EmailSendingException : Exception
        {
            public EmailSendingException(string message) : base(message) { }
        }
    }
}

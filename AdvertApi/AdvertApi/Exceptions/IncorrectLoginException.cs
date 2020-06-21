using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.Exceptions
{
    public class IncorrectLoginException : Exception
    {
        public IncorrectLoginException() { }

        public IncorrectLoginException(string message) : base(message) { }

        public IncorrectLoginException(string message, Exception innerException) : base(message, innerException) { }
    }
}

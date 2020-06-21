using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertApi.Exceptions
{
    public class LoginExistException : Exception
    {
        public LoginExistException() { }

        public LoginExistException(string message) : base(message) { }

        public LoginExistException(string message, Exception innerException) : base(message, innerException) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AdvertApi.Exceptions
{
    [Serializable]
    internal class RefreshTokenIncorrectException : Exception
    {
        public RefreshTokenIncorrectException() { }

        public RefreshTokenIncorrectException(string message) : base(message) { }

        public RefreshTokenIncorrectException(string message, Exception innerException) : base(message, innerException) { }

        protected RefreshTokenIncorrectException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AdvertApi.Exceptions
{
    [Serializable]
    internal class BuildingsNotExistException : Exception
    {
        public BuildingsNotExistException() { }

        public BuildingsNotExistException(string message) : base(message) { }

        public BuildingsNotExistException(string message, Exception innerException) : base(message, innerException) { }

        protected BuildingsNotExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

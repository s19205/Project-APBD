﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AdvertApi.Exceptions
{
    [Serializable]
    internal class BuildingsNotNearException : Exception
    {
        public BuildingsNotNearException() { }

        public BuildingsNotNearException(string message) : base(message) { }

        public BuildingsNotNearException(string message, Exception innerException) : base(message, innerException) { }

        protected BuildingsNotNearException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

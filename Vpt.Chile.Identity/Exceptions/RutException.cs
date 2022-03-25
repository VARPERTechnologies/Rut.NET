using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Vpt.Chile.Identity.Exceptions
{
    [Serializable]
    public class RutException : Exception
    {
        public RutException()
        {
        }

        public RutException(string message) : base(message)
        {
        }

        public RutException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RutException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

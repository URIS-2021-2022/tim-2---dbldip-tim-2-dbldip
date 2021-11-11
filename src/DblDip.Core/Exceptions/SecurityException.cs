using System;
using System.Runtime.Serialization;

namespace DblDip.Core.Exceptions
{
    [Serializable]
    public class SecurityException: Exception
    {
        public SecurityException(string message) : base(message) { }

        protected SecurityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}

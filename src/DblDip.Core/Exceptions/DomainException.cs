using System;
using System.Runtime.Serialization;

namespace DblDip.Core.Exceptions
{
    [Serializable]
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }

        protected DomainException(SerializationInfo info, StreamingContext context)
        {
            //
        }
    }
}

using System;
using System.Runtime.Serialization;

namespace Infrastructure.Material.Exceptions
{
    public class DuplicateBatchException : Exception
    {
        public DuplicateBatchException()
        {
        }

        public DuplicateBatchException(string message) : base(message)
        {
        }

        public DuplicateBatchException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DuplicateBatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
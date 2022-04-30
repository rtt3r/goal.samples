using System.Runtime.Serialization;

namespace Goal.Demo.Api.Exceptions
{
    [Serializable]
    public class DomainViolationException : ApplicationException
    {
        public DomainViolationException()
        {
        }

        public DomainViolationException(string message)
            : base(message)
        {
        }

        public DomainViolationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected DomainViolationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}

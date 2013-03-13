using System;
using System.Runtime.Serialization;

namespace Articles.Model.ExceptionHandlingUsingPolicies.ExternalService
{
    [Serializable]
    public class TransientException : Exception
    {
        public TransientException()
        {
        }

        public TransientException(string message) : base(message)
        {
        }

        public TransientException(string message, Exception inner) : base(message, inner)
        {
        }

        protected TransientException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
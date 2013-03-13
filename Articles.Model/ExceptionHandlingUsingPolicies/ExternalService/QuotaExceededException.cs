using System;
using System.Runtime.Serialization;

namespace Articles.Model.ExceptionHandlingUsingPolicies.ExternalService
{
    [Serializable]
    public class QuotaExceededException : Exception
    {
        public QuotaExceededException()
        {
        }

        public QuotaExceededException(string message) : base(message)
        {
        }

        public QuotaExceededException(string message, Exception inner) : base(message, inner)
        {
        }

        protected QuotaExceededException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
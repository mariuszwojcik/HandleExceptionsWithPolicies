using System;
using System.Runtime.Serialization;

namespace Articles.Model.ExceptionHandlingUsingPolicies.ExternalService
{
    [Serializable]
    public class DuplicateTitleException : Exception
    {
        public string Title { get; set; }

        public DuplicateTitleException()
        {
        }

        public DuplicateTitleException(string message) : base(message)
        {
        }

        public DuplicateTitleException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DuplicateTitleException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
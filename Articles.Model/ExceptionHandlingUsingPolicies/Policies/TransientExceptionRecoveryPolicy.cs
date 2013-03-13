using System;
using System.Threading;
using Articles.Model.ExceptionHandlingUsingPolicies.ExternalService;

namespace Articles.Model.ExceptionHandlingUsingPolicies.Policies
{
    public class TransientExceptionRecoveryPolicy<TResult> : IRecoverablePolicy<TResult>
    {
        public int BackoffTime { get; set; }
        public int RetriesCount { get; set; }

        public TransientExceptionRecoveryPolicy()
        {
            BackoffTime = 50;
            RetriesCount = 5;
        }

        public TResult Execute(Func<TResult> operation)
        {
            return Execute(operation, 1);
        }

        private TResult Execute(Func<TResult> operation, int counter)
        {
            try
            {
                return operation.Invoke();
            }
            catch (TransientException)
            {
                if (counter > RetriesCount)
                    throw;

                Thread.Sleep((counter * counter) * BackoffTime);
                return Execute(operation, counter + 1);
            }
        }
    }
}
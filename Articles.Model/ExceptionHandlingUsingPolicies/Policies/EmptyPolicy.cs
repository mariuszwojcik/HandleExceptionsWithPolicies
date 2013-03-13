using System;

namespace Articles.Model.ExceptionHandlingUsingPolicies.Policies
{
    public class EmptyPolicy<TResult> : IRecoverablePolicy<TResult>
    {
        public TResult Execute(Func<TResult> operation)
        {
            return operation.Invoke();
        }
    }
}
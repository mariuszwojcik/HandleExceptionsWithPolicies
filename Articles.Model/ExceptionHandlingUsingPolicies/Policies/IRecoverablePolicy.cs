using System;

namespace Articles.Model.ExceptionHandlingUsingPolicies.Policies
{
    public interface IRecoverablePolicy<TResult>
    {
        TResult Execute(Func<TResult> operation);
    }
}
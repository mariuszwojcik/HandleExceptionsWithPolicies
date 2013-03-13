using System;
using System.Collections.Generic;

namespace Articles.Model.ExceptionHandlingUsingPolicies.Policies
{
    public class CompositeRecoverablePolicy<TResult> : IRecoverablePolicy<TResult>
    {
        private readonly List<IRecoverablePolicy<TResult>> _policies;

        public CompositeRecoverablePolicy(IEnumerable<IRecoverablePolicy<TResult>> policies)
        {
            _policies = new List<IRecoverablePolicy<TResult>>(policies);
        }

        public TResult Execute(Func<TResult> operation)
        {
            var chainedPolicies = operation;

            foreach (var policy in _policies)
            {
                var localOperation = chainedPolicies;
                var currentPolicy = policy;
                chainedPolicies = () => currentPolicy.Execute(localOperation);
            }

            return chainedPolicies.Invoke();
        }
    }
}
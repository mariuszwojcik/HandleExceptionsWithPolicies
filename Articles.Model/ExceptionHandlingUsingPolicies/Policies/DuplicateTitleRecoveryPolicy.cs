using System;
using Articles.Model.ExceptionHandlingUsingPolicies.ExternalService;

namespace Articles.Model.ExceptionHandlingUsingPolicies.Policies
{
    public class DuplicateTitleRecoveryPolicy : IRecoverablePolicy<long>
    {
        private readonly IExternalService _service;

        public DuplicateTitleRecoveryPolicy(IExternalService service)
        {
            _service = service;
        }

        public long Execute(Func<long> operation)
        {
            try
            {
                return operation.Invoke();
            }
            catch (DuplicateTitleException e)
            {
                var article = _service.GetArticleByTitle(e.Title);
                return article.Id;
            }
        }

        
    }
}
using Articles.Model.ExceptionHandlingUsingPolicies.ExternalService;
using Articles.Model.ExceptionHandlingUsingPolicies.Policies;

namespace Articles.Model.ExceptionHandlingUsingPolicies
{
    public class ExternalServiceFacadeWithPolicies : IExternalServiceFacade
    {
        private readonly IExternalService _service;

        public IRecoverablePolicy<long> CreateArticlePolicy { get; set; }

        public ExternalServiceFacadeWithPolicies(IExternalService service)
        {
            _service = service;
            CreateArticlePolicy = new EmptyPolicy<long>();
        }

        public long CreateArticle(string title, string author, string body)
        {
            return CreateArticlePolicy.Execute(() => _service.CreateArticle(title, author, body));
        }

        public Article GetArticle(string title)
        {
            throw new System.NotImplementedException();
        }
    }
}
using Articles.Model.ExceptionHandlingUsingPolicies.ExternalService;

namespace Articles.Model.ExceptionHandlingUsingPolicies
{
    public interface IExternalServiceFacade
    {
        long CreateArticle(string title, string author, string body);
        Article GetArticle(string title);
    }
}
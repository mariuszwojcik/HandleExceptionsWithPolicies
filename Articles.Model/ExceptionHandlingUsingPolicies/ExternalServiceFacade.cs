using System;
using System.Threading;
using Articles.Model.ExceptionHandlingUsingPolicies.ExternalService;

namespace Articles.Model.ExceptionHandlingUsingPolicies
{
    public class ExternalServiceFacade : IExternalServiceFacade
    {
        private readonly IExternalService _service;

        public ExternalServiceFacade(IExternalService service)
        {
            _service = service;
        }

        public long CreateArticle(string title, string author, string body)
        {
            long articleId;

            try
            {
                articleId =  _service.CreateArticle(title, author, body);
            }
            catch (DuplicateTitleException e)
            {
                var article = GetArticle(title);
                articleId = article.Id;
            }
            catch (TransientException e)
            {
                Thread.Sleep(50);
                return CreateArticle(title, author, body);
            }
            
            return articleId;
        }

        public Article GetArticle(string title)
        {
            Article result;

            try
            {
                result = _service.GetArticleByTitle(title);
            }
            catch (NotFoundException e)
            {
                result = null;
            }
            catch(TransientException e)
            {
                Thread.Sleep(50);
                result = GetArticle(title);
            }

            return result;
        }
    }
}
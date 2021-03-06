﻿namespace Articles.Model.ExceptionHandlingUsingPolicies.ExternalService
{
    public interface IExternalService
    {
        /// <summary>
        /// Creates new article.
        /// </summary>
        /// <param name="title">Title of the article. The title must be unique.</param>
        /// <param name="author">Author of the article.</param>
        /// <param name="body">Text of the article.</param>
        /// <returns>Autogenerated ID for newly created article.</returns>
        /// <exception cref="DuplicateTitleException">Thrown when there already is an article with given title.</exception>
        /// <exception cref="QuotaExceededException">Thrown when daily quota for operations has been exceeded.</exception>
        /// <exception cref="TransientException">Thrown when there was a transient exception on the server.</exception>
        long CreateArticle(string title, string author, string body);
        
        /// <summary>
        /// Gets the article by title.
        /// Throws <see cref="NotFoundException"/> if article doesn't exist.
        /// </summary>
        /// <param name="title">Title of the article to get.</param>
        /// <returns>An article.</returns>
        /// <exception cref="TransientException">Thrown when there was a transient exception on the server.</exception>
        /// <exception cref="NotFoundException">Thrown when article with given title doesn't exists.</exception>
        Article GetArticleByTitle(string title);
    }
}
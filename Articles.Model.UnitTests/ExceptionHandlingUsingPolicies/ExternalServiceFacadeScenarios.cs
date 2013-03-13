using Articles.Model.ExceptionHandlingUsingPolicies;
using Articles.Model.ExceptionHandlingUsingPolicies.ExternalService;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;

namespace Articles.Model.UnitTests.ExceptionHandlingUsingPolicies
{
    [TestFixture]
    public class ExternalServiceFacadeScenarios
    {
        private const string Title = "article";
        private const string Author = "me";
        private const string Body = "content";
        private IExternalService _service;
        private IExternalServiceFacade _facade;

        [SetUp]
        public void TestSetUp()
        {
            _service = MockRepository.GenerateMock<IExternalService>();

            _facade = new ExternalServiceFacade(_service);
        }

        [Test]
        public void WhenCreatingArticleThenForwardsToTheService()
        {
            _service.Expect(s => s.CreateArticle(Title, Author, Body)).Repeat.Once().Return(1);

             _facade.CreateArticle(Title, Author, Body);

            _service.VerifyAllExpectations();
        }

        [Test]
        public void WhenCreatingArticleThenReturnsArticleIdReturnedFromService()
        {
            const long articleId = 98;

            _service.Stub(s => s.CreateArticle(Title, Author, Body)).Repeat.Once().Return(articleId);

            var result = _facade.CreateArticle(Title, Author, Body);

            Assert.AreEqual(articleId, result);
        }

        [Test]
        public void WhenThereIsDuplicateTitleExceptionThenGetsArticleIdFromService()
        {
            const long articleId = 95;

            _service.Expect(s => s.CreateArticle(Title, Author, Body)).Repeat.Once().Throw(new DuplicateTitleException());
            _service.Expect(s => s.GetArticleByTitle(Title)).Repeat.Once().Return(new Article(articleId, Title, Author, Body));

            var result = _facade.CreateArticle(Title, Author, Body);

            Assert.AreEqual(articleId, result);
        }

        [Test]
        public void WhenThereIsTransientExceptionThenRetriesOperation()
        {
            const long articleId = 96;

            _service.Expect(s => s.CreateArticle(Title, Author, Body)).Repeat.Twice().Throw(new TransientException());
            _service.Expect(s => s.CreateArticle(Title, Author, Body)).Repeat.Once().Return(articleId);

            var result = _facade.CreateArticle(Title, Author, Body);

            Assert.AreEqual(articleId, result);
            _service.VerifyAllExpectations();
        }

        [Test]
        public void WhenThereAreTransientAndDuplicateTitleExceptionsThenReturnsArticleId()
        {
            const long articleId = 35;

            _service.Expect(s => s.CreateArticle(Title, Author, Body)).Repeat.Twice().Throw(new TransientException());
            _service.Expect(s => s.CreateArticle(Title, Author, Body)).Repeat.Once().Throw(new DuplicateTitleException());
            _service.Expect(s => s.GetArticleByTitle(Title)).Repeat.Once().Return(new Article(articleId, Title, Author, Body));

            var result = _facade.CreateArticle(Title, Author, Body);

            Assert.AreEqual(articleId, result);
            _service.VerifyAllExpectations();
        }
    }
}
using System;
using System.Collections.Generic;
using Articles.Model.ExceptionHandlingUsingPolicies;
using Articles.Model.ExceptionHandlingUsingPolicies.ExternalService;
using NUnit.Framework;
using Rhino.Mocks;


namespace Articles.Model.UnitTests.ExceptionHandlingUsingPolicies
{
    [TestFixture]
    public class WhenCreatingNewArticle
    {
        private IExternalService _service;
        private IExternalServiceFacade _facade;

        [SetUp]
        public void TestSetUp()
        {
            _service = MockRepository.GenerateMock<IExternalService>();

            _facade = new ExternalServiceFacade(_service);
        }

        [Test]
        public void ThenDelegatesToExternalService()
        {
            const string title = "test";
            const string author = "me";
            const string content = "content";
            const int id = 1;

            _service.Stub(s => s.CreateArticle(title, author, content)).IgnoreArguments().Return(id);

            var articleId = _facade.CreateArticle(title, author, content);

            Assert.AreEqual(id, articleId);
        }

        [Test]
        public void AndThereWasATransienErrorThenRetries()
        {
            const string title = "test";
            const string author = "me";
            const string content = "content";
            const int id = 1;


            _service.Expect(s => s.CreateArticle(title, author, content)).Throw(new TransientException()).Repeat.Once();
            _service.Expect(s => s.CreateArticle(title, author, content)).Return(id).Repeat.Once();

            var articleId = _facade.CreateArticle(title, author, content);

            Assert.AreEqual(id, articleId);
            _service.VerifyAllExpectations();
        }

        [Test]
        public void ThereAreFewConsecutiveTransientExcetpionsThenRetiresUntilSuccessful()
        {
            const string title = "test";
            const string author = "me";
            const string content = "content";
            const int id = 1;


            _service.Expect(s => s.CreateArticle(title, author, content)).Throw(new TransientException()).Repeat.Times(3);
            _service.Expect(s => s.CreateArticle(title, author, content)).Return(id).Repeat.Once();

            var articleId = _facade.CreateArticle(title, author, content);

            Assert.AreEqual(id, articleId);
            _service.VerifyAllExpectations();
        }

        [Test]
        public void AndThereIsDuplicateTitleExceptionThenGetsArticle()
        {
            const string title = "test";
            const string author = "me";
            const string content = "content";
            const int id = 1;

            _service.Expect(s => s.CreateArticle(title, author, content)).Throw(new DuplicateTitleException());
            _service.Expect(s => s.GetArticleByTitle(title)).Return(new Article(id ,title, author, content));

            var articleId = _facade.CreateArticle(title, author, content);

            Assert.AreEqual(id, articleId);
            _service.VerifyAllExpectations();
        }

        [Test]
        public void AndThereIsTransientExceptionAndThenDuplicateTitleExceptionThenGetsArticle()
        {
            const string title = "test";
            const string author = "me";
            const string content = "content";
            const int id = 1;

            _service.Expect(s => s.CreateArticle(title, author, content)).Throw(new TransientException()).Repeat.Once();
            _service.Expect(s => s.CreateArticle(title, author, content)).Throw(new DuplicateTitleException()).Repeat.Once();
            _service.Expect(s => s.GetArticleByTitle(title)).Return(new Article(id, title, author, content));

            var articleId = _facade.CreateArticle(title, author, content);

            Assert.AreEqual(id, articleId);
            _service.VerifyAllExpectations();
        }
    }
}
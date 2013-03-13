using Articles.Model.ExceptionHandlingUsingPolicies.ExternalService;
using Articles.Model.ExceptionHandlingUsingPolicies.Policies;
using NUnit.Framework;
using Rhino.Mocks;

namespace Articles.Model.UnitTests.ExceptionHandlingUsingPolicies.Policies
{
    [TestFixture]
    public class WhenHandlingDuplicateTitleException
    {
        private IExternalService _service;
        private IRecoverablePolicy<long> _policy;
        private const string Title = "test";
        private const string Author = "me";
        private const string Body = "content";

        [SetUp]
        public void TestSetUp()
        {
            _service = MockRepository.GenerateMock<IExternalService>();

            _policy = new DuplicateTitleRecoveryPolicy(_service);
        }

        [Test]
        public void ThenGetsArticleFromService()
        {
            _service.Expect(s => s.CreateArticle(Title, Author, Body)).Throw(new DuplicateTitleException { Title = Title}).Repeat.Once();
            _service.Expect(s => s.GetArticleByTitle(Title)).Return(new Article(93, Title, Author, Body)).Repeat.Once();

            _policy.Execute(() => _service.CreateArticle(Title, Author, Body));

            _service.VerifyAllExpectations();
        }

        [Test]
        public void ThenReturnsIdOfExistingArticle()
        {
            const long articleId = 56;
            _service.Expect(s => s.CreateArticle(Title, Author, Body)).Throw(new DuplicateTitleException { Title = Title }).Repeat.Once();
            _service.Expect(s => s.GetArticleByTitle(Title)).Return(new Article(articleId, Title, Author, Body)).Repeat.Once();

            var result = _policy.Execute(() => _service.CreateArticle(Title, Author, Body));

            Assert.AreEqual(articleId, result);
        }
    }
}
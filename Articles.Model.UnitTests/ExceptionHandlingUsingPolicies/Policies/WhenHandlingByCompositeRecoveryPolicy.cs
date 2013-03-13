using System;
using System.Linq;
using Articles.Model.ExceptionHandlingUsingPolicies.ExternalService;
using Articles.Model.ExceptionHandlingUsingPolicies.Policies;
using NUnit.Framework;
using Rhino.Mocks;

namespace Articles.Model.UnitTests.ExceptionHandlingUsingPolicies.Policies
{
    [TestFixture]
    public class WhenHandlingByCompositeRecoveryPolicy
    {
        private IExternalService _service;
        private IRecoverablePolicy<long> _policy;

        [SetUp]
        public void TestSetUp()
        {
            _service = MockRepository.GenerateMock<IExternalService>();
            var policies = new IRecoverablePolicy<long>[]
                               {
                                   new TransientExceptionRecoveryPolicy<long>(),
                                   new DuplicateTitleRecoveryPolicy(_service)
                               };

            _policy = new CompositeRecoverablePolicy<long>(policies);
        }

        [Test]
        public void WhenExecutingThenChainsPolicies()
        {
            var policies = new[]
                               {
                                   new TestPolicy(1),
                                   new TestPolicy(2),
                                   new TestPolicy(3)
                               }.ToList();
            var policy = new CompositeRecoverablePolicy<int>(policies);

            policy.Execute(() => 3);

            Assert.That(policies.All(i => i.Executed));
        }

        [Test]
        public void WhenFailsWithTransientAndDuplicateTitleThenReturnsArticleId()
        {
            const string title = "test";
            const string author = "me";
            const string content = "content";
            const int id = 47;

            _service.Expect(s => s.CreateArticle(title, author, content)).Throw(new TransientException()).Repeat.Once();
            _service.Expect(s => s.CreateArticle(title, author, content)).Throw(new DuplicateTitleException{Title = title}).Repeat.Once();
            _service.Expect(s => s.GetArticleByTitle(title)).Return(new Article(id, title, author, content));

            var articleId = _policy.Execute(() => _service.CreateArticle(title, author, content));

            Assert.AreEqual(id, articleId);
            _service.VerifyAllExpectations();
        }
    }

    internal class TestPolicy : IRecoverablePolicy<int>
    {
        private readonly int _id;

        public bool Executed { get; private set; }

        public TestPolicy(int id)
        {
            _id = id;
        }

        public int Execute(Func<int> operation)
        {
            Console.WriteLine("executing policy {0}", _id);
            Executed = true;
            return operation.Invoke();
        }
    }
}
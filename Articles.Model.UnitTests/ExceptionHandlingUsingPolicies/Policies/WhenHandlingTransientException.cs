using System;
using System.Diagnostics;
using Articles.Model.ExceptionHandlingUsingPolicies.ExternalService;
using Articles.Model.ExceptionHandlingUsingPolicies.Policies;
using NUnit.Framework;
using Rhino.Mocks;

namespace Articles.Model.UnitTests.ExceptionHandlingUsingPolicies.Policies
{
    [TestFixture]
    public class WhenHandlingTransientException
    {
        private IExternalService _service;
        private IRecoverablePolicy<long> _policy;
        private const int PolicyBackoffTime = 100;
        private const int PolicyRetriesCount = 3;
        private const string Title = "test";
        private const string Author = "me";
        private const string Body = "content";

        [SetUp]
        public void TestSetUp()
        {
            _service = MockRepository.GenerateMock<IExternalService>();

            _policy = new TransientExceptionRecoveryPolicy<long>
                          {
                              BackoffTime = PolicyBackoffTime,
                              RetriesCount = PolicyRetriesCount
                          };
        }

        [Test]
        public void ThenRetries()
        {
            _service.Expect(s => s.CreateArticle(Title, Author, Body)).Throw(new TransientException()).Repeat.Once();
            _service.Expect(s => s.CreateArticle(Title, Author, Body)).Return(1).Repeat.Once();

            _policy.Execute(() => _service.CreateArticle(Title, Author, Body));

            _service.VerifyAllExpectations();
        }

        [Test]
        public void ThenUsersExponentialBackOffBetweenRetries()
        {
            _service.Expect(s => s.CreateArticle(Title, Author, Body)).Throw(new TransientException()).Repeat.Times(3);
            _service.Expect(s => s.CreateArticle(Title, Author, Body)).Return(1).Repeat.Once();

            var watch = Stopwatch.StartNew();
            _policy.Execute(() => _service.CreateArticle(Title, Author, Body));
            watch.Stop();

            const int expectedExecutionTime = PolicyBackoffTime + (PolicyBackoffTime * 4) + (PolicyBackoffTime * 9);
            Assert.That(watch.Elapsed, Is.EqualTo(TimeSpan.FromMilliseconds(expectedExecutionTime)).Within(TimeSpan.FromMilliseconds(50)));
            _service.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException(typeof(TransientException))]
        public void AndFailsGivenTimesConsecutivelyThenFails()
        {
            _service.Expect(s => s.CreateArticle(Title, Author, Body)).Throw(new TransientException()).Repeat.Times(PolicyRetriesCount + 1);

            _policy.Execute(() => _service.CreateArticle(Title, Author, Body));
        }
    }
}
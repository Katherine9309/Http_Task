using NUnit.Framework;
using System.Net;
using TechTalk.SpecFlow;
using UserService;

namespace Test.StepDefinitions
{
    [Binding]
    public sealed class WalletServiceAssertion
    {
        private readonly DataContext _context;
        public WalletServiceAssertion(DataContext context)
        {
            _context = context;
        }
        [Then(@"the balance response status is ([^']*)")]
        public void ThenTheBalanceResponseStatusIs(string codeResponse)
        {
            HttpStatusCode codeExpected = (HttpStatusCode)System.Enum.Parse(typeof(HttpStatusCode), codeResponse, true);
            Assert.That(_context.GetBalanceStatusResponse.StatusCode, Is.EqualTo(codeExpected));
        }

        [Then(@"the balance is '([^']*)'")]
        public void ThenTheBalanceIs(int balance)
        {
            Assert.That(_context.GetBalanceStatusResponse.Body, Is.EqualTo(0));
        }

        [Then(@"the reverse transaction response status is ([^']*)")]
        public void ThenTheReverseTransactionResponseStatusIs(string codeResponse)
        {
            HttpStatusCode codeExpected = (HttpStatusCode)System.Enum.Parse(typeof(HttpStatusCode), codeResponse, true);
            Assert.That(_context.ReverseTransactionStatusResponse.StatusCode, Is.EqualTo(codeExpected));
        }

        [Then(@"the second reverse transaction response status is ([^']*)")]
        public void ThenTheSecondReverseTransactionResponseStatusIs(string codeResponse)
        {
            HttpStatusCode codeExpected = (HttpStatusCode)System.Enum.Parse(typeof(HttpStatusCode), codeResponse, true);
            Assert.That(_context.SecondReverseTransactionStatusResponse.StatusCode, Is.EqualTo(codeExpected));
        }

        [Then(@"the charge response status is ([^']*)")]
        public void ThenTheChargeResponseStatusIs(string codeResponse)
        {
            HttpStatusCode codeExpected = (HttpStatusCode)System.Enum.Parse(typeof(HttpStatusCode), codeResponse, true);
            Assert.That(_context.ChargeUserStatusResponse.StatusCode, Is.EqualTo(codeExpected));
        }

        [Then(@"the message charge response is ""(.*)""")]
        public void ThenTheMessageResponseIs(string messageResponse)
        {
            Assert.AreEqual(messageResponse, _context.ChargeUserStatusResponse.Content);
        }

        [Then(@"the balance is (.*)")]
        public void ThenTheBalanceIs(Double overalBalance)
        {
            Assert.That(_context.GetBalanceStatusResponse.Body, Is.EqualTo(overalBalance));
        }

    }
}
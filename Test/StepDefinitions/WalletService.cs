using Newtonsoft.Json.Linq;
using NUnit.Framework.Interfaces;
using TechTalk.SpecFlow;
using UserService;
using UserService.Client;
using UserService.Utils;
using WalletService.Client;
using WalletService.Utils;

namespace Test.StepDefinitions
{
    [Binding]
    public sealed class WalletService
    {
        private readonly WalletServiceClient _walletService = new WalletServiceClient();
        private readonly WalletGenerator _walletGenerator = new WalletGenerator();
        private readonly UserServiceClient _registerUser = new UserServiceClient();
        private readonly UserGenerator _userGenerator = new UserGenerator();
        private readonly DataContext _context;

        public WalletService(DataContext context)
        {
            _context = context;
        }

        [When(@"a request to get the balance is made")]
        public async Task WhenARequestToGetTheBalanceIsMade()
        {
            _context.GetBalanceStatusResponse = await _walletService.GetBalance(_context.UserId);
        }

        [Given(@"a new ramdon Id transaction")]
        public void GivenANewRamdonIdTransaction()
        {
            _context.UserIdTransaction = _walletGenerator.GenerateTransactionId();
        }

        [Given(@"a request to revert the transaction is made")]
        [When(@"a request to revert the transaction is made")]
        public async Task WhenARequestToRevertTheTransactionIsMade()
        {
            _context.ReverseTransactionStatusResponse = await _walletService.RevertTransaction(_context.UserIdTransaction);
            _context.SecondUserIdTransaction = _context.ReverseTransactionStatusResponse.Body;
        }

        [When(@"charge a ramdon  amount")]
        [Given(@"charge a ramdon amount")]
        public async Task GivenChargeAndRamdonAmount()
        {
             var chargeRequest = _walletGenerator.GenerateChargeRequest(_context.UserId);
            _context.ChargeUserStatusResponse = await _walletService.ChargeUser(chargeRequest);
            _context.UserIdTransaction = _context.ChargeUserStatusResponse.Body;
        }

        [When(@"a request to revert the last revert transaction")]
        public async Task WhenARequestToRevertTheLastRevertTransaction()
        {
            _context.SecondReverseTransactionStatusResponse = await _walletService.RevertTransaction(_context.SecondUserIdTransaction);
        }

        [Given(@"an (.*) is charged to the user")]
        [When(@"an (.*) is charged to the user")]
        public async Task WhenAnIsChargedToTheUser(Double amount)
        {
            var chargeRequest = _walletGenerator.GenerateChargeRequest(_context.UserId, amount);
            _context.ChargeUserStatusResponse = await _walletService.ChargeUser(chargeRequest);
            _context.UserIdTransaction = _context.ChargeUserStatusResponse.Body;
        }

        [Given(@"a request to change the user status to ([^']*) is made")]
        [Given(@"change to active user status to (.*) is made")]
        public async Task GivenARequestToChangeTheUserStatusToBoolstateIsMade(bool status)
        {
            await _registerUser.SetUserStatus(_context.UserId, status);
        }

        [Given(@"I made charge transactions for the following (.+)")]
        [When(@"I made charge transactions for the following (.+)")]
        public async Task WhenIMadeChargeTransactionsForTheFollowing(string amounts)
        {
     
            var amountArray= amounts.Split(',');
            foreach (var item in amountArray)
            {
                var chargeRequest = _walletGenerator.GenerateChargeRequest(_context.UserId, Double.Parse(item));
                _context.ChargeUserStatusResponse = await _walletService.ChargeUser(chargeRequest);
            }
        }

        [Given(@"an charge of (.*) plus (.*) is charged")]
        public async Task GivenAnChargeOfPlusIsCharged(double amount, double value)
        {
            var chargeRequest = _walletGenerator.GenerateChargeRequest(_context.UserId, amount+value);
            _context.ChargeUserStatusResponse = await _walletService.ChargeUser(chargeRequest);
        }

        [Given(@"charge (.*) minus (.*) to the user")]
        public async Task GivenAnMinusIsChargedToTheUser(double amount, double value)
        {
            var chargeRequest = _walletGenerator.GenerateChargeRequest(_context.UserId, -amount - value);
            _context.ChargeUserStatusResponse = await _walletService.ChargeUser(chargeRequest);
        }

        [When(@"an minus (.*) is charged")]
        public async Task WhenAnMinusIsCharged(double amount)
        {
            var chargeRequest = _walletGenerator.GenerateChargeRequest(_context.UserId, -amount);
            _context.ChargeUserStatusResponse = await _walletService.ChargeUser(chargeRequest);
        }


    }
}
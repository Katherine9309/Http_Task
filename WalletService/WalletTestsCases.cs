using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Drawing;
using System.Globalization;
using System.Net;
using UserService.Client;
using UserService.Models.Responses;
using UserService.Utils;
using WalletService.Client;
using WalletService.Models.Requests;
using WalletService.Utils;

namespace WalletService
{
    public class Tests
    {

        private readonly UserServiceClient _registerUser = new UserServiceClient();
        private readonly UserGenerator _userGenerator = new UserGenerator();
        private readonly WalletServiceClient _walletService = new WalletServiceClient();
        private readonly WalletGenerator _walletGenerator = new WalletGenerator();



        [TestCase(new double[] { 10, 20, 30, -10, -20, -30 }, 0, "OK")] // test 4
        [TestCase(new double[] { 10, 20, 30, -20, -30, -9.99 }, 0.01, "OK")] // test 5
        [TestCase(new double[] { 10, 20, 30, -20, -30, -10.01 }, 10, "InternalServerError")] // test 6
        [TestCase(new double[] { 5000000, 4000000,1000000, -0.01 }, 9999999.99, "OK")] // test 7
        [TestCase(new double[] { 5000000, 4000000, 500000, 500000 }, 10000000, "OK")] // test 8
        public async Task KatheGetBalance_SetNewUserAndGetBalance_StatusCodeInternal(double[] amounts, double balance, string codeResponse)
        {
            //precondition
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            await _registerUser.SetUserStatus(response.Body, true);
            var chargeResponse = new CommonResponse<Guid>();
            //Action
            foreach (var item in amounts) {
                var chargeRequest = _walletGenerator.GenerateChargeRequest(response.Body, item);
                chargeResponse = await _walletService.ChargeUser(chargeRequest);
            }

            var balanceRequest = await _walletService.GetBalance(response.Body);
            HttpStatusCode codeExpected = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), codeResponse, true);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(balanceRequest.Body, Is.EqualTo(balance));
                Assert.That(chargeResponse.StatusCode, Is.EqualTo(codeExpected));
            });
        }


        [Test] // test 1,2
        public async Task GetBalance_SetNewUserAndGetBalance_StatusCodeInternalError()
        {
            //precondition
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);

            //Action
            var response2 = await _walletService.GetBalance(response.Body);

            //Assert
            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }


        [Test] // test 3
        public async Task GetBalance_SetNotExistingIdUserAndGetBalance_StatusError()
        {
            //precondition
            var randomId = _userGenerator.GenerateId();

            //Action
            var response2 = await _walletService.GetBalance(randomId);

            //Assert
            Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Test] // test 15
        public async Task GetBalance_CreateUserAndChangeStatusToActiveAndGetBalance_StatusOK()
        {

            //precondition
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            await _registerUser.SetUserStatus(response.Body, true);

            //Action
            var response2 = await _walletService.GetBalance(response.Body);

            //Assert  
            Assert.Multiple(() =>
            {
                Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response2.Body, Is.EqualTo(0));
            });
        }


        [Test] // test 38
        public async Task ReverseTransaction_CreateUserAndChangeStatusToActiveChargeAmountAndMakeDoubleReverse_StatusOK()
        {
            //precondition
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            await _registerUser.SetUserStatus(response.Body, true);
            var chargeRequest = _walletGenerator.GenerateChargeRequest(response.Body);

            //Action
            var chargeResponse = await _walletService.ChargeUser(chargeRequest);
            var responseFirstReverse = await _walletService.RevertTransaction(chargeResponse.Body);
            var responseSecondReverse = await _walletService.RevertTransaction(responseFirstReverse.Body);
            //Assert  
            Assert.Multiple(() =>
            {
      
                Assert.That(chargeResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(responseFirstReverse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(responseSecondReverse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            });
           
        }


        [Test] // test 33
        public async Task Revert_RevertTransactionForWrongIdTransaction_StatusNotFound()
        {
            //precondition
            var idTransaction = _walletGenerator.GenerateTransactionId();

            //Action
            var response = await _walletService.RevertTransaction(idTransaction);

            //Assert  
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test] // test 43
        public async Task Charge_CreateNotActiveUserAndCharge_StatusNotActiveUser()
        {
            //precondition
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            var request2 = _walletGenerator.GenerateChargeRequest(response.Body);
            //Action
            var response2 = await _walletService.ChargeUser(request2);
            //Assert  
            Assert.Multiple(() =>
            {
                Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
                Assert.That(response2.Content, Is.EqualTo("not active user"));
            });
        }

        [Test] // test 44
        public async Task Charge_NoexistingUserCharge_StatusNotActiveUser()
        {
            //precondition
            var request = _walletGenerator.GenerateChargeRequest();
            //Action
            var response = await _walletService.ChargeUser(request);
            //Assert  
            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
                Assert.That(response.Content, Is.EqualTo("not active user"));
            });
        }

        [TestCase(10000000.01)] // test 47, test 37
        public async Task ChargeAndRevert_CreateActiveUserWithBalanceZeroAndChargeAmountOverTenMillion_StatusInternalError(double amount)
        {

            //precondition
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            await _registerUser.SetUserStatus(response.Body, true);
            var request2 = _walletGenerator.GenerateChargeRequest(response.Body, amount);

            //Action
            var chargeResponse = await _walletService.ChargeUser(request2);
            var revertResponse = await _walletService.RevertTransaction(chargeResponse.Body);

            //Assert  
            Assert.Multiple(() =>
            {
                Assert.That(chargeResponse.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
                Assert.That(chargeResponse.Content, Is.EqualTo($"After this charge balance could be '{amount}', maximum user balance is '10000000'"));
                Assert.That(revertResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });

        }



        [TestCase(0.001, "Amount value must have precision 2 numbers after dot")] //test 48
        [TestCase(0, "Amount cannot be '0'")] //test 46

        public async Task Charge_CreateUserAndBalanceZeroChargeDifferentsAmounts_StatusExepcion(double amount, string message)
        {
            //precondition
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            await _registerUser.SetUserStatus(response.Body, true);
            //Action
            var request2 = _walletGenerator.GenerateChargeRequest(response.Body, amount);
            var response2 = await _walletService.ChargeUser(request2);

            //Assert 
            Assert.Multiple(()=>
            {
                Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
                Assert.That(response2.Content, Is.EqualTo(message));
            });
        }

        [TestCase(20)] // test 56
        [TestCase(60)]
        public async Task GetBalance_CreateUserAndBalanceIsAmountPlusTenAndChargeAmount_StatusOK(double amount)
        {
            //precondition
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            await _registerUser.SetUserStatus(response.Body, true);
            var request2 = _walletGenerator.GenerateChargeRequest(response.Body, amount + 10);
            await _walletService.ChargeUser(request2);
            //Action
            var response2 = await _walletService.GetBalance(response.Body);
            var request3 = _walletGenerator.GenerateChargeRequest(response.Body, amount);
            var response3 = await _walletService.ChargeUser(request3);
            var response4 = await _walletService.GetBalance(response.Body);

            //Assert  

            Assert.Multiple(() =>
            {
                Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response2.Body, Is.EqualTo(amount + 10));
                Assert.That(response3.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response4.Body, Is.EqualTo(amount + amount + 10));
            });
        }

        [TestCase(20)] // test 45
        [TestCase(100)]
        public async Task GetBalance_CreateUserAndBalanceIsNChargeMinusNMinusZeroPointZeroZeroOne_StatusOK(double amount)
        {

            //precondition
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            await _registerUser.SetUserStatus(response.Body, true);
            var request2 = _walletGenerator.GenerateChargeRequest(response.Body, amount);
            await _walletService.ChargeUser(request2);
            //Action
            var response2 = await _walletService.GetBalance(response.Body);
            var chargeRequest = _walletGenerator.GenerateChargeRequest(response.Body, -amount - 0.01);
            var chargeResponse = await _walletService.ChargeUser(chargeRequest);
            var response4 = await _walletService.GetBalance(response.Body);


            //Assert  
            Assert.Multiple(() =>
            {
                Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response2.Body, Is.EqualTo(amount));
                Assert.That(chargeResponse.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
                Assert.That(response4.Body, Is.EqualTo(amount));
            });
        }

   
        [TestCase(0.01, 0.01,"OK")] //test 10, test 49, test 34
        [TestCase(9999999.99,9999999.99,"OK")] //test 12, test 39 
        [TestCase(10000000,10000000,"OK")] // test 13 , test 36,  test 16

        public async Task Charge_CreateActiveUserInitialBalanceZeroAndChargeAmount_StatusOK(double firstCharge, double overalBalance, string codeResponse)
        {
            //precondition
            var requestUser = _userGenerator.GenerateCreateUserRequest();
            var newUser = await _registerUser.CreateUser(requestUser);
            await _registerUser.SetUserStatus(newUser.Body, true);


            //Action
            var chargeRequest = _walletGenerator.GenerateChargeRequest(newUser.Body, firstCharge);
            var chargeResponse = await _walletService.ChargeUser(chargeRequest);
            var firstBalanceResponse = await _walletService.GetBalance(newUser.Body);
            var revertResponse = await _walletService.RevertTransaction(chargeResponse.Body);
            var secondBalanceResponse = await _walletService.GetBalance(newUser.Body);
        
            HttpStatusCode codeExpected = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), codeResponse, true);

            //Assert  
            Assert.Multiple(() =>
            {
                Assert.That(chargeResponse.StatusCode, Is.EqualTo(codeExpected));
                Assert.That(firstBalanceResponse.Body, Is.EqualTo(overalBalance));
                Assert.That(firstBalanceResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(revertResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(secondBalanceResponse.Body, Is.EqualTo(0));

            });
        }

        [TestCase(-30, 0, "InternalServerError")] //test 55
        public async Task Charge_CreateUserAndBalanceZeroAndChargeAmount_StatusInternalServerErrorAndExepcion(double amount, double balance,string codeResponse)
        {
            //precondition
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            await _registerUser.SetUserStatus(response.Body, true);
            //Action
            var request2 = _walletGenerator.GenerateChargeRequest(response.Body, amount);
            var response2 = await _walletService.ChargeUser(request2);
            HttpStatusCode codeExpected = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), codeResponse, true);
            //Assert 
            Assert.Multiple(() =>
            {
                Assert.That(response2.StatusCode, Is.EqualTo(codeExpected));
                Assert.That(response2.Content, Is.EqualTo($"User have '0', you try to charge '{amount.ToString("0.0")}'."));
            });
        }


        [TestCase(-0.01, 0, "InternalServerError")] //test 11, test 35,test 50
        [TestCase(-10000000.01, 0, "InternalServerError")] //test 14
        public async Task Charge_CreateActiveUserInitialBalanceZeroAndChargeNegativeAmount_StatusInternalServerError(double firstCharge, double overalBalance, string codeResponse)
        {
            //precondition
            var requestUser = _userGenerator.GenerateCreateUserRequest();
            var newUser = await _registerUser.CreateUser(requestUser);
            await _registerUser.SetUserStatus(newUser.Body, true);


            //Action
            var chargeRequest = _walletGenerator.GenerateChargeRequest(newUser.Body, firstCharge);
            var chargeResponse = await _walletService.ChargeUser(chargeRequest);
            var firstBalanceResponse = await _walletService.GetBalance(newUser.Body);
            var revertResponse = await _walletService.RevertTransaction(chargeResponse.Body);
            var secondBalanceResponse = await _walletService.GetBalance(newUser.Body);

            HttpStatusCode codeExpected = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), codeResponse, true);
        //Assert  
        Assert.Multiple(() =>
            {
                Assert.That(chargeResponse.StatusCode, Is.EqualTo(codeExpected));
                Assert.That(chargeResponse.Content, Is.EqualTo($"User have '0', you try to charge '{firstCharge}'."));
                Assert.That(firstBalanceResponse.Body, Is.EqualTo(overalBalance));
                Assert.That(firstBalanceResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(revertResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                Assert.That(secondBalanceResponse.Body, Is.EqualTo(0));

            });
        }

        [TestCase(20, 20, "OK",0)] //test 53
        [TestCase(-20, 0, "InternalServerError",20)] //test54
        public async Task Charge_CreateActiveUserInitialBalanceNAndChargeMinusN_StatusOK(double firstCharge,double balance, string codeResponse, double overallBalance)
        {
            //precondition
            var requestUser = _userGenerator.GenerateCreateUserRequest();
            var newUser = await _registerUser.CreateUser(requestUser);
            await _registerUser.SetUserStatus(newUser.Body, true);
            var chargeRequest = _walletGenerator.GenerateChargeRequest(newUser.Body, firstCharge);
            var chargeResponse = await _walletService.ChargeUser(chargeRequest);
            var firstBalanceResponse = await _walletService.GetBalance(newUser.Body);

            //Action
            var chargeRequest2 = _walletGenerator.GenerateChargeRequest(newUser.Body, -firstCharge);
            var chargeResponse2 = await _walletService.ChargeUser(chargeRequest2);
            var firstBalanceResponse2 = await _walletService.GetBalance(newUser.Body);
           
            HttpStatusCode codeExpected = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), codeResponse, true);

            //Assert  
            Assert.Multiple(() =>
            {
                Assert.That(chargeResponse.StatusCode, Is.EqualTo(codeExpected));
                Assert.That(firstBalanceResponse.Body, Is.EqualTo(balance));
                Assert.That(firstBalanceResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                Assert.That(chargeResponse2.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(firstBalanceResponse2.Body, Is.EqualTo(overallBalance));
                Assert.That(firstBalanceResponse2.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            });
        }
    }
}
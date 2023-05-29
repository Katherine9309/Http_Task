using System.Drawing;
using System.Net;
using UserService.Client;
using UserService.Utils;
using WalletService.Client;
using WalletService.Utils;

namespace WalletService
{
    public class Tests
    {

        private readonly UserServiceClient _registerUser = new UserServiceClient();
        private readonly UserGenerator _userGenerator = new UserGenerator();
        private readonly WalletServiceClient _walletService = new WalletServiceClient();
        private readonly WalletGenerator _walletGenerator = new WalletGenerator();

        [Test] // test 1,2
        public async Task GetBalance_SetNewUserAndGetBalance_StatusCodeInternalError()
        {
            //precondition
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);

            //Action
            var response2 = await _walletService.GetBalance(response.Body);
        
            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response2.StatusCode);
             
        }


        [Test] // test 3
        public async Task GetBalance_SetNotExistingIdUserAndGetBalance_StatusError()
        {
            //precondition
            var randomId = _userGenerator.GenerateId();

            //Action
            var response2 = await _walletService.GetBalance(randomId);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response2.StatusCode);

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
            Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
        }

        [Test] // test 43
        public async Task Charge_CreateNotActiveUserAndCharge_StatusNotActiveUser()
        {

            //precondition
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            var request2 =  _walletGenerator.GenerateChargeRequest(response.Body);

            //Action
            
            var response2 = await _walletService.ChargeUser(request2);

            //Assert  
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, response2.StatusCode);
                Assert.AreEqual("not active user", response2.Content);

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
                Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
                Assert.AreEqual("not active user", response.Content);
            });

        }

        [Test] // test 46
        public async Task Charge_CreateActiveUserAndChargeZero_StatusInternalError()
        {

            //precondition
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            await _registerUser.SetUserStatus(response.Body, true);
            var request2 = _walletGenerator.GenerateChargeRequest(response.Body,0);

            //Action
            var response2 = await _walletService.ChargeUser(request2);

            //Assert  
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, response2.StatusCode);
                Assert.AreEqual("Amount cannot be '0'", response2.Content);

            });

        }
        [Test] // test 47, test 14
        public async Task Charge_CreateActiveUserWithBalanceZeroAndChargeTenMillionAndOne_StatusInternalError()
        {

            //precondition
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            await _registerUser.SetUserStatus(response.Body, true);
            var request2 = _walletGenerator.GenerateChargeRequest(response.Body, 10000000.01);

            //Action
            var response2 = await _walletService.ChargeUser(request2);

            //Assert  
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, response2.StatusCode);
                Assert.AreEqual("After this charge balance could be '10000000.01', maximum user balance is '10000000'" , response2.Content);

            });

        }

        [Test] // test 48
        public async Task Charge_CreateActiveUserWithBalanceZeroAndChargeZeroPointZeroZeroOne_StatusInternalError()
        {

            //precondition
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            await _registerUser.SetUserStatus(response.Body, true);
            var request2 = _walletGenerator.GenerateChargeRequest(response.Body, 0.001);

            //Action
            var response2 = await _walletService.ChargeUser(request2);

            //Assert  
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, response2.StatusCode);
                Assert.AreEqual("Amount value must have precision 2 numbers after dot", response2.Content);

            });

        }

        [Test] // test 49, test 10, test 34
        public async Task Charge_CreateActiveUserWithBalanceZeroAndChargeZeroPointZeroOne_StatusOk()
        {

            //precondition
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            await _registerUser.SetUserStatus(response.Body, true);
            var request2 = _walletGenerator.GenerateChargeRequest(response.Body, 0.01);

            //Action
            var response2 = await _walletService.ChargeUser(request2);
            var response3 = await _walletService.GetBalance(response.Body);
            var response4 = await _walletService.RevertTransaction(response2.Body);
            //Assert  

            
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
                Assert.AreEqual(HttpStatusCode.OK, response3.StatusCode);
                Assert.AreEqual(0.01, response3.Body);
                Assert.AreEqual(HttpStatusCode.OK, response4.StatusCode);
            });
        }

        [Test] // test 50
        public async Task Charge_CreateActiveUserWithBalanceZeroAndChargeMinusZeroPointZeroOne_StatusInternalError()
        {

            //precondition
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            await _registerUser.SetUserStatus(response.Body, true);
            var request2 = _walletGenerator.GenerateChargeRequest(response.Body, -0.01);

            //Action
            var response2 = await _walletService.ChargeUser(request2);
            var response3 = await _walletService.GetBalance(response.Body);

            //Assert  

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, response2.StatusCode);
                Assert.AreEqual("User have '0', you try to charge '-0.01'.", response2.Content);
                Assert.AreEqual(HttpStatusCode.OK, response3.StatusCode);
          
            });

        }


        [Test] // test 12, test 39,test 16
        public async Task Charge_CreateActiveUserWithBalanceZeroAndChargenineMillionNineHundredNinetyNineThousandNineHundredNinetyNinePointNineNine_StatusOK()
        {
            //precondition
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            await _registerUser.SetUserStatus(response.Body, true);
            var request2 = _walletGenerator.GenerateChargeRequest(response.Body, 9999999.99);

            //Action

            var response2 = await _walletService.ChargeUser(request2);
            var response3 = await _walletService.GetBalance(response.Body);
            var response4 = await _walletService.RevertTransaction(response2.Body);
            var response5 = await _walletService.GetBalance(response.Body);


            //Assert  
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
                Assert.AreEqual(9999999.99, response3.Body);
                Assert.AreEqual(HttpStatusCode.OK, response3.StatusCode);
                Assert.AreEqual(HttpStatusCode.OK, response4.StatusCode);
                Assert.AreEqual(0, response5.Body);

            });
        }

        [Test] // test 13,test 16, test 36
        public async Task Charge_CreateActiveUserWithBalanceZeroAndChargeTenMillion_StatusOK()
        {
            //precondition
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            await _registerUser.SetUserStatus(response.Body, true);
            var request2 = _walletGenerator.GenerateChargeRequest(response.Body, 10000000);

            //Action
            var response2 = await _walletService.ChargeUser(request2);
            var response3 = await _walletService.GetBalance(response.Body);
            var response4 = await _walletService.RevertTransaction(response2.Body);
            var response5 = await _walletService.GetBalance(response.Body);
            //Assert  

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
                Assert.AreEqual(10000000, response3.Body);
                Assert.AreEqual(HttpStatusCode.OK, response3.StatusCode);
                Assert.AreEqual(HttpStatusCode.OK, response4.StatusCode);
                Assert.AreEqual(0, response5.Body);
                Assert.AreEqual(HttpStatusCode.OK, response5.StatusCode);

            });
        }


        [Test] // test 33
        public async Task Revert_RevertTransactionWithWrongId_StatusNotFound()
        {
            //precondition
       
            var idTransaction = _walletGenerator.GenerateTransactionId();

            //Action

            var response = await _walletService.RevertTransaction(idTransaction);

            //Assert  
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test] // test 33
        public async Task Balance_RevertTransactionAndGetBalance_StatusNotFound()
        {
            //precondition

            var idTransaction = _walletGenerator.GenerateTransactionId();

            //Action

            var response = await _walletService.RevertTransaction(idTransaction);

            //Assert  
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test] // test 53
        public async Task Charge_CreateUserAndBalanceNAndChargeMinusNWhereNIsOverZero_StatusOk()
        {

            //precondition
            double amount = 100;
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            await _registerUser.SetUserStatus(response.Body, true);
            var request2 = _walletGenerator.GenerateChargeRequest(response.Body, amount);
            await _walletService.ChargeUser(request2);
            //Action
            var response2 = await _walletService.GetBalance(response.Body);
            var request3 = _walletGenerator.GenerateChargeRequest(response.Body, -amount);
            var response3 = await _walletService.ChargeUser(request3);
            var response4 = await _walletService.GetBalance(response.Body);

            //Assert  

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
                Assert.AreEqual(amount, response2.Body);
                Assert.AreEqual(HttpStatusCode.OK, response3.StatusCode);
                Assert.AreEqual(0, response4.Body);
            });
        }

        [Test] // test 54
        public async Task Charge_CreateUserAndBalanceNAndChargeMinusNWhereNIsBelowZero_StatusOk()
        {

            //precondition
            double amount = -100;
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            await _registerUser.SetUserStatus(response.Body, true);
            var request2 = _walletGenerator.GenerateChargeRequest(response.Body, amount);
            await _walletService.ChargeUser(request2);
            //Action
            var response2 = await _walletService.GetBalance(response.Body);
            var request3 = _walletGenerator.GenerateChargeRequest(response.Body, -amount);
            var response3 = await _walletService.ChargeUser(request3);
            var response4 = await _walletService.GetBalance(response.Body);

            //Assert  

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
                Assert.AreEqual(amount, response2.Body);
                Assert.AreEqual(HttpStatusCode.OK, response3.StatusCode);
                Assert.AreEqual(0, response4.Body);
            });
        }

        [Test] // test 55
        public async Task Charge_CreateUserAndBalanceZeroChargeMinusThirty_StatusExepcion()
        {

            //precondition
            double amount = -30;
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            await _registerUser.SetUserStatus(response.Body, true);
            //Action
            var request2 = _walletGenerator.GenerateChargeRequest(response.Body, amount);
            var response2 = await _walletService.ChargeUser(request2);
            
            //Assert 
            Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
            
        }

        [Test] // test 56
        public async Task GetBalance_CreateUserAndBalanceIsNPlusTenAndChargeN_BalanceIsTwiceNplusTen()
        {

            //precondition
            double amount = 100;
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            await _registerUser.SetUserStatus(response.Body, true);
            var request2 = _walletGenerator.GenerateChargeRequest(response.Body, amount+10);
            await _walletService.ChargeUser(request2);
            //Action
            var response2 = await _walletService.GetBalance(response.Body);
            var request3 = _walletGenerator.GenerateChargeRequest(response.Body, amount);
            var response3 = await _walletService.ChargeUser(request3);
            var response4 = await _walletService.GetBalance(response.Body);

            //Assert  

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
                Assert.AreEqual(amount+10, response2.Body);
                Assert.AreEqual(HttpStatusCode.OK, response3.StatusCode);
                Assert.AreEqual(amount+amount+10, response4.Body);
            });
        }

        [Test] // test 45
        public async Task GetBalance_CreateUserAndBalanceIsNChargeMinusNMinusZeroPointZeroZeroOne_StstusOK()
        {

            //precondition
            double amount = 100;
            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);
            await _registerUser.SetUserStatus(response.Body, true);
            var request2 = _walletGenerator.GenerateChargeRequest(response.Body, amount);
            await _walletService.ChargeUser(request2);
            //Action
            var response2 = await _walletService.GetBalance(response.Body);
            var request3 = _walletGenerator.GenerateChargeRequest(response.Body, -amount-0.01);
            var response3 = await _walletService.ChargeUser(request3);
            var response4 = await _walletService.GetBalance(response.Body);

            //Assert  

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
                Assert.AreEqual(amount, response2.Body);
                Assert.AreEqual(HttpStatusCode.OK, response3.StatusCode);
                Assert.AreEqual(amount+(-amount-0.01), response4.Body);
            });
        }

    }
}
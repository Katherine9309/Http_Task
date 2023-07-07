using System.Net;
using UserService.Client;
using UserService.Enum;
using UserService.Models.Requests;
using UserService.Utils;

namespace UserService
{
 

    [TestFixture]
    public class RegisterUserMethod
    {
        private readonly UserServiceClient _registerUser = new UserServiceClient();
        private readonly UserGenerator _userGenerator = new UserGenerator();
        //case 1
        [Test]
        public async Task RegisterUser_CreateUserWithEmptyFields_StatusCodeIsOK()
        {
            //precondition

            var request = _userGenerator.GenerateCreateUserRequest(string.Empty, string.Empty);

            //Action
            var response = await _registerUser.CreateUser(request);

            //Assert
            Console.WriteLine(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        //case 2
        [Test]
        public async Task RegisterUser_CreateUserWithNullFields_StatusCodeIsOK()
        {
            //precondition

            var request = _userGenerator.GenerateCreateUserRequest(null, null);

            //Action
            var response = await _registerUser.CreateUser(request);

            //Assert
            Console.WriteLine(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        //case 3 and 7
        [Test]
        public async Task RegisterUser_CreateUserWithDigitsAndUpperCaseOnFields_StatusCodeIsOK()
        {
            //precondition

            var request = _userGenerator.GenerateCreateUserRequest();
            Console.WriteLine(request.FirstName);
            //Action
            var response = await _registerUser.CreateUser(request);

            //Assert
            Console.WriteLine(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        //case 4, case 6
        [Test]
        public async Task RegisterUser_CreateUserWithHundredLengthOnFields_StatusCodeIsOK()
        {
            //precondition

            var request = _userGenerator.GenerateCreateRandomUserRequest(100);
            Console.WriteLine(request.FirstName);
            Console.WriteLine(request.LastName);
            //Action
            var response = await _registerUser.CreateUser(request);

            //Assert

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        //case 4, case 6
        [Test]
        public async Task RegisterUser_CreateUserWithIdAutoincremented_StatusCodeIsOK()
        {
            //precondition

            var request1 = _userGenerator.GenerateCreateUserRequest();
            var response1 = await _registerUser.CreateUser(request1);
            var request2 = _userGenerator.GenerateCreateUserRequest();
            //Action
            var response2 = await _registerUser.CreateUser(request2);

            //Assert
         

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, response1.StatusCode);
                Assert.IsTrue(response2.Body > response1.Body + 1);

            });
        }
    }

    [TestFixture]
    public class GetAndSetStatusMethod
    {
        private readonly UserServiceClient _registerUser = new UserServiceClient();
        private readonly UserGenerator _userGenerator = new UserGenerator();


        [Test] //Case 1
        public async Task GetUserStatus_SetStatusToNotExistingUser_StatusIsNotFound()
        {
            //precondition

            var id = _userGenerator.GenerateId();

            //Action
            var response = await _registerUser.GetUserStatus(id);

            //Assert

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
                Assert.AreEqual("cannot find user with this id", response.Content);
            });
        }
        [Test] //Case 11
        public async Task GetUserStatus_CreateUserAndCheckStatus_StatusIsInactive()
        {
            //precondition

            var request = _userGenerator.GenerateCreateUserRequest();
            var response = await _registerUser.CreateUser(request);

            //Action
            var response2 = await _registerUser.GetUserStatus(response.Body);
            Console.WriteLine(response2.Body);
            Console.WriteLine(response2.Content);
            //Assert
            Assert.AreEqual(false, response2.Body);

        }

        [Test] //Case_12 GetUseStatus and Case 15 SetUserStatus
        public async Task GetUserStatus_CreateUserAndChangeStatusToActive_StatusIsActive()
        {
            //precondition

            var response = await _registerUser.CreateUser(_userGenerator.GenerateCreateUserRequest());

            //Action
            await _registerUser.SetUserStatus(Convert.ToInt32(response.Body), true);
            var response2 = await _registerUser.GetUserStatus(response.Body);
            Console.WriteLine(response2.Body);
            Console.WriteLine(response2.Content);

            //Assert
            Assert.AreEqual(true, response2.Body);

        }

        [Test] //Case_14  SetUserStatus
        public async Task SetUserStatus_ChangeStatusToActiveToNotExistingUser_StatusNotFound()
        {
            //precondition

            var id = _userGenerator.GenerateId();

            //Action
            var response = await _registerUser.SetUserStatus(id, true);

            Console.WriteLine(response.Body);
            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
                Assert.AreEqual("cannot find user with this id", response.Content);
            });

        }

        [Test] //Case_13 and Case_16   
        public async Task GetUserStatus_CreateUserAndChangeStatusToActiveThenToInactive_StatusIsInactive()
        {
            //precondition

            var response = await _registerUser.CreateUser(_userGenerator.GenerateCreateUserRequest());
            await _registerUser.SetUserStatus(response.Body, true);
            //Action
            await _registerUser.SetUserStatus(response.Body, false);
            var response2 = await _registerUser.GetUserStatus(response.Body);


            //Assert
            Assert.AreEqual(false, response2.Body);

        }

        [Test] //Case_17
        public async Task GetUserStatus_CreateUserAndChangeStatusToActiveDoItTwice_StatusIsActive()
        {
            //precondition

            var response = await _registerUser.CreateUser(_userGenerator.GenerateCreateUserRequest());
            await _registerUser.SetUserStatus(response.Body, true);
            await _registerUser.SetUserStatus(response.Body, false);
            //Action
            await _registerUser.SetUserStatus(response.Body, true);
            var response2 = await _registerUser.GetUserStatus(response.Body);


            //Assert
            Assert.AreEqual(true, response2.Body);

        }


        [Test] //Case_18
        public async Task GetUserStatus_CreateUserAndSetStatusToInactive_StatusIsInactive()
        {
            //precondition

            var response = await _registerUser.CreateUser(_userGenerator.GenerateCreateUserRequest());

            //Action
            await _registerUser.SetUserStatus(response.Body, false);
            var response2 = await _registerUser.GetUserStatus(response.Body);


            //Assert
            Assert.AreEqual(false, response2.Body);

        }

        [Test] //Case_19
        public async Task GetUserStatus_CreateUserAndSetStatusToActive_StatusIsActive()
        {
            //precondition

            var response = await _registerUser.CreateUser(_userGenerator.GenerateCreateUserRequest());
            await _registerUser.SetUserStatus(response.Body, true);

            //Action
            await _registerUser.SetUserStatus(response.Body, true);
            var response2 = await _registerUser.GetUserStatus(response.Body);


            //Assert
            Assert.AreEqual(true, response2.Body);

        }
     }

    [TestFixture]
    public class DeletMethod
    {

        private readonly UserServiceClient _registerUser = new UserServiceClient();
        private readonly UserGenerator _userGenerator = new UserGenerator();


        [Test] //Case_21
        public async Task DeleteUser_DeleteNotExistingUser_StatusIsNotFound()
        {
            //precondition
          
            var response = await _registerUser.CreateUser(_userGenerator.GenerateCreateUserRequest());

            //Action
            await _registerUser.DeleteUser(response.Body);
            var response2 = await _registerUser.DeleteUser(response.Body);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.InternalServerError, response2.StatusCode);
                Assert.AreEqual("Sequence contains no elements", response2.Content);
            });
        }

        [Test] //Case_20
        public async Task DeleteUser_DeleteNotActiveUser_StatusDeleted()
        {
            //precondition
            var response = await _registerUser.CreateUser(_userGenerator.GenerateCreateUserRequest());

            //Action
            var response2 = await _registerUser.DeleteUser(response.Body);

            //Assert
          
           Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
       
        }

    }


}
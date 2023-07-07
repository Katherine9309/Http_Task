using System.Net;
using TechTalk.SpecFlow;
using UserService.Client;
using UserService.Utils;

namespace UserService.Models.Steps
{
    [Binding]
    public sealed class UserSteps
    {
        private readonly UserServiceClient _registerUser = new UserServiceClient();
        private readonly UserGenerator _userGenerator = new UserGenerator();
        private readonly DataContext _context;
        public UserSteps(DataContext context)
        {
            _context = context;
        }

        [Given(@"a empty first and last name")]
        public void GivenAEmptyFirstAndLastName()
        {
            _context.CreateUserRequest = _userGenerator.GenerateCreateUserRequest(string.Empty, string.Empty);
        }


        [Given(@"a null firts and last name")]
        public void GivenANullFirtsAndLastName()
        {
            _context.CreateUserRequest = _userGenerator.GenerateCreateUserRequest(null, null);
        }

        [Given(@"a digits and upper case on first name and last name")]
        public void GivenADigitsAndUpperCaseOnFirstNameAndLastName()
        {
            _context.CreateUserRequest = _userGenerator.GenerateCreateUserRequest();
        }

        [Given(@"a first name and last name with hundred length on fields")]
        public void GivenAFirstNameAndLastNameWithHundredLengthOnFields()
        {
            _context.CreateUserRequest = _userGenerator.GenerateCreateRandomUserRequest(100);
        }

        [Given(@"a new user is created")]
        public async Task GivenANewUserIsCreated()
        {
            var requestUser = _userGenerator.GenerateCreateUserRequest();
            _context.CreateUserStatusResponse = await _registerUser.CreateUser(requestUser);
            _context.UserId = _context.CreateUserStatusResponse.Body;
            Console.WriteLine(_context.CreateUserStatusResponse);
            Console.WriteLine(_context.UserId);
        }

        [Given(@"a new ramdon Id")]
        public void GivenANewRamdonId()
        {
            _context.UserId = _userGenerator.GenerateId();
        }

        [When(@"the user is deleted")]
        [Given(@"the user is deleted")]
        [When(@"a new request to delete the user is made")]
        public async Task GivenTheUserIsDeleted()
        {
            _context.DeleteUserStatusResponse = await _registerUser.DeleteUser(_context.CreateUserStatusResponse.Body);
        }

      

        [When(@"a second user is created")]
        public async Task WhenASecondUserIsCreated()
        {
            var requestUser = _userGenerator.GenerateCreateUserRequest();
            _context.CreateSecondUserStatusResponse = await _registerUser.CreateUser(requestUser);
            _context.SecondUserId = _context.CreateSecondUserStatusResponse.Body;
        }


        [When(@"sending a request to create a new user")]
        public async Task WhenCreateANewUser()
        {
            _context.CreateUserStatusResponse = await _registerUser.CreateUser(_context.CreateUserRequest);
        }
       

        [When(@"a request is made it to check user status")]
        public async Task WhenARequestIsMadeItToCheckUserStatus()
        {
            _context.GetUserStatusResponse = await _registerUser.GetUserStatus(_context.UserId); 
        }

        [When(@"a request to change the user status to '(.*)' is made")]
        [Given(@"a request to change the user status to '(.*)' is made")]
        public async Task WhenARequestToChangeTheUserStatusIsMade(bool status )
        {
            await _registerUser.SetUserStatus(_context.UserId, status);
            _context.GetUserStatusResponse = await _registerUser.GetUserStatus(_context.UserId);

        }



    }
}
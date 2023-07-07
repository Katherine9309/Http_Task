using NUnit.Framework;
using System.Net;
using TechTalk.SpecFlow;



namespace UserService.Models.Steps
{
    [Binding]
    public sealed class UserStepsAssertion
    {
        private readonly DataContext _context;

        public UserStepsAssertion(DataContext context)
        {
            _context = context;
        }

        [Then(@"the user response status code is OK")]
        public void ThenCreateNewUserResponseStatusCodeIsOK()
        {
            Assert.AreEqual(HttpStatusCode.OK, _context.CreateUserStatusResponse.StatusCode);
        }

        [Then(@"the Id of the second user is greater")]
        public void ThenTheIdOfTheSecondUserIsGreater()
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.OK, _context.CreateUserStatusResponse.StatusCode);
                Assert.IsTrue(_context.SecondUserId > _context.UserId);

            });
        }

        [Then(@"the check status response is '(.*)'")]
        public void ThenTheCheckStatusResponseIsNotFound(string codeResponse)
        {


            HttpStatusCode codeExpected = (HttpStatusCode)System.Enum.Parse(typeof(HttpStatusCode), codeResponse, true);

            Assert.Multiple(() =>
            {
                Assert.That(_context.GetUserStatusResponse.StatusCode, Is.EqualTo(codeExpected));
                Assert.AreEqual("cannot find user with this id", _context.GetUserStatusResponse.Content);
            });
        }


        [Then(@"the user status is '(.*)'")]
        public void ThenTheUserStatusIs(bool codeResponse)
        {
            Assert.AreEqual(codeResponse, _context.GetUserStatusResponse.Body);
        }


        [Then(@"the set user status request is '([^']*)'")]
        public void ThenTheSetUserStatusRequestIs(string codeResponse)
        {
            HttpStatusCode codeExpected = (HttpStatusCode)System.Enum.Parse(typeof(HttpStatusCode), codeResponse, true);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(HttpStatusCode.NotFound, _context.SetUserStatusResponse.StatusCode);
                Assert.AreEqual("cannot find user with this id", _context.SetUserStatusResponse.Content);
            });
        }

        [Then(@"the delete user response status code is '([^']*)'")]
        public void ThenTheDeleteUserResponseStatusCodeIs(string codeResponse)
        {
            HttpStatusCode codeExpected = (HttpStatusCode)System.Enum.Parse(typeof(HttpStatusCode), codeResponse, true);
            Assert.AreEqual(codeExpected, _context.DeleteUserStatusResponse.StatusCode);
        }

        [Then(@"the message response was '([^']*)'")]
        public void ThenTheMessageResponseWas(string messageResponse)
        {
            Assert.AreEqual(messageResponse, _context.DeleteUserStatusResponse.Content);

        }

    }

}
    

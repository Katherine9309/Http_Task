using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Client;

namespace UserService
{
    [SetUpFixture]
    public class SetUpFixture
    {
        [OneTimeSetUp]
      // public void OneTimeSetUp() { 
      // 
      // }

        [OneTimeTearDown]
        public async Task OneTimeTearDown() {

            var client = new UserServiceClient();
            var tasks = TestDataStorage
                .GetAllIds()
                .Select(id => client.DeleteUser(id));

            await Task.WhenAll(tasks);  
        }

        public static class TestDataStorage
        {
            private static readonly ConcurrentBag<int> _storage = new ConcurrentBag<int>();
            public static void Add(int id)
            {
                _storage.Add(id);   
            }
            public static IEnumerable<int> GetAllIds()
            {

               return _storage.ToArray();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Models.Requests;

namespace UserService.Utils
{
    public class UserGenerator
    {
        public RegisterUserRequest GenerateCreateUserRequest()
        {
            return GenerateCreateUserRequest("KCV" + Guid.NewGuid().ToString().ToUpper().Replace("-", ""));
        }

        public RegisterUserRequest GenerateCreateUserRequest(string firstName)
        {
            return GenerateCreateUserRequest(firstName, "Cucunuba" + Guid.NewGuid().ToString());
        }

        public RegisterUserRequest GenerateCreateUserRequest(string firstName, string lastName)
        {
            return new RegisterUserRequest()
            {
                FirstName = firstName ?? "",
                LastName = lastName ?? "",
            };
        }


        public RegisterUserRequest GenerateCreateRandomUserRequest(int length)
        {
            const string _string = "abcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()/{";
            Random random = new Random();   

            StringBuilder firstName = new StringBuilder();
            StringBuilder lastName = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(_string.Length);
                lastName.Append(_string[index]);
                index = random.Next(_string.Length);
                firstName.Append(_string[index]);
            }

            return GenerateCreateUserRequest(firstName.ToString(), lastName.ToString());
        }


        public int GenerateId() {
            Random random = new Random();
            int randomNumber = random.Next(0,10000000);
            return randomNumber;
        } 

    }
}

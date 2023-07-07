using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Models.Requests;
using WalletService.Models.Requests;

namespace WalletService.Utils
{
    public class WalletGenerator
    {
        
     
        public ChargeRequest GenerateChargeRequest()
        {
            Random random = new Random();

            return GenerateChargeRequest(random.Next(0, 100) , (random.Next((int)(0 * 100), (int)(100000.0 * 100)) / 100.0));
        }

        public ChargeRequest GenerateChargeRequest(int userId)
        {
            Random random = new Random();
            return GenerateChargeRequest(userId, (random.Next((int)(0 * 100), (int)(100000.0 * 100)) / 100.0));
        }

        public ChargeRequest  GenerateChargeRequest(int userId, double amount)
        {
            return new ChargeRequest()
            {
                UserId = userId,
                Amount = amount,
            };
        }

        public Guid GenerateTransactionId()
        {
            return Guid.NewGuid();
        }

    }
}

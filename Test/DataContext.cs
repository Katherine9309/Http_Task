using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using UserService.Models.Requests;
using UserService.Models.Responses;
using WalletService.Client;
using WalletService.Models.Requests;

namespace UserService
{
    public  class DataContext
    {
     
        public int UserId;
        public int SecondUserId;
        public CommonResponse<int> CreateUserStatusResponse;
        public RegisterUserRequest CreateUserRequest;
        public CommonResponse<int> CreateSecondUserStatusResponse;
        public CommonResponse<object> GetUserStatusResponse;
        public CommonResponse<object> SetUserStatusResponse;
        public CommonResponse<object> DeleteUserStatusResponse;
        public CommonResponse<object> GetBalanceStatusResponse;
        public Guid UserIdTransaction;
        public CommonResponse<Guid> ReverseTransactionStatusResponse;
        public CommonResponse<Guid> ChargeUserStatusResponse;
        public Guid SecondUserIdTransaction;
        public CommonResponse<Guid> SecondReverseTransactionStatusResponse;
    }
}

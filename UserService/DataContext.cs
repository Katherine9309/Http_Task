
using UserService.Models.Requests;
using UserService.Models.Responses;

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
        public Guid UserIdTransaction;
        public CommonResponse<Guid> ReverseTransactionStatusResponse;
        public CommonResponse<Guid> SecondReverseTransactionStatusResponse;
        public CommonResponse<Guid>ChargeStatusResponse;
    }
}

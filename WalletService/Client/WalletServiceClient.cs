
using Newtonsoft.Json;
using System.Text;
using UserService.CatalogTests.Models.Responses;
using UserService.Models.Requests;
using UserService.Models.Responses;
using UserService.Utils;
using WalletService.Models.Requests;

namespace WalletService.Client
{
    public class WalletServiceClient
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _baseUrl = " https://walletservice-uat.azurewebsites.net";

     public async Task<CommonResponse<Guid>> ChargeUser(ChargeRequest request)
     {
         var httpRequestMessage = new HttpRequestMessage
         {
             Method = HttpMethod.Post,
             RequestUri = new Uri($"{_baseUrl}//Balance/Charge"),
             Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
         };
   
         HttpResponseMessage response = await _httpClient.SendAsync(httpRequestMessage);
   
         return await response.ToCommonResponse<Guid>();
   
     }

       public async Task <CommonResponse<object>> RevertTransaction(Guid userId)
       {
           var getUserStatusRequest = new HttpRequestMessage
           {
               Method = HttpMethod.Put,
               RequestUri = new Uri($"{_baseUrl}/Balance/RevertTransaction?transactionId={userId}")
           };
 
           HttpResponseMessage response = await _httpClient.SendAsync(getUserStatusRequest);
 
           return await response.ToCommonResponse<object>();
       }

        public async Task<CommonResponse<object>> GetBalance(int userId)
        {
            var getBalanceRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUrl}/Balance/GetBalance?userId={userId}")
            };

            HttpResponseMessage response = await _httpClient.SendAsync(getBalanceRequest);

            return await response.ToCommonResponse<object>();
        }


       // public async Task<CommonResponse<object>> DeleteUser(int userId)
       // {
       //     var getProductInfoRequest = new HttpRequestMessage
       //     {
       //         Method = HttpMethod.Delete,
       //         RequestUri = new Uri($"{_baseUrl}/Register/DeleteUser?userId={userId}")
       //     };
       //
       //     HttpResponseMessage response = await _httpClient.SendAsync(getProductInfoRequest);
       //
       //     return await response.ToCommonResponse<object>();
       // }
    }
}

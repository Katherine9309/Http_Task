using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UserService.CatalogTests.Models.Responses;
using UserService.Models.Requests;
using UserService.Models.Responses;
using UserService.Utils;

namespace UserService.Client
{
    public class UserServiceClient
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _baseUrl = " https://userservice-uat.azurewebsites.net";

        public async Task<CommonResponse<object>> CreateUser(RegisterUserRequest request)
        {
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_baseUrl}/Register/RegisterNewUser"),
                Content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
            };

            HttpResponseMessage response = await _httpClient.SendAsync(httpRequestMessage);

            return await response.ToCommonResponse<object>();

        }

      public async Task <CommonResponse<object>> SetUserStatus(int userId, bool userStatus)
      {
          var getUserStatusRequest = new HttpRequestMessage
          {
              Method = HttpMethod.Put,
              RequestUri = new Uri($"{_baseUrl}/UserManagement/SetUserStatus?userId={userId}&newStatus={userStatus}")
          };
    
          HttpResponseMessage response = await _httpClient.SendAsync(getUserStatusRequest);
    
          return await response.ToCommonResponse<object>();
      }

        public async Task<CommonResponse<object>> GetUserStatus(int userId)
        {
            var getProductInfoRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{_baseUrl}/UserManagement/GetUserStatus?userId={userId}")
            };

            HttpResponseMessage response = await _httpClient.SendAsync(getProductInfoRequest);

            return await response.ToCommonResponse<object>();
        }


        public async Task<CommonResponse<object>> DeleteUser(int userId)
        {
            var getProductInfoRequest = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"{_baseUrl}/Register/DeleteUser?userId={userId}")
            };

            HttpResponseMessage response = await _httpClient.SendAsync(getProductInfoRequest);

            return await response.ToCommonResponse<object>();
        }
    }
}

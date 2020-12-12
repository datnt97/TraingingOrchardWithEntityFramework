using eTweb.ViewModels.Common;
using eTweb.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eTweb.AdminApp.Services
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserApiClient(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await client.PostAsync("/api/users/authenticate", httpContent);
            var result = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiErrorResult<string>>(result);

            return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(result);
            ;
        }

        public async Task<ApiResult<UserViewModel>> GetUserById(Guid id)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var httpResponse = await client.GetAsync($"/api/users/{id}");
            var result = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiErrorResult<UserViewModel>>(result);

            return JsonConvert.DeserializeObject<ApiSuccessResult<UserViewModel>>(result);
        }

        public async Task<ApiResult<PagedResult<UserViewModel>>> GetUsersPaging(GetUsersRequest request)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var httpResponse = await client.GetAsync(
                    $"/api/users/paging?" +
                    $"pageIndex={request.PageIndex}&" +
                    $"pageSize={request.PageSize}&" +
                    $"keyword={request.Keyword}");

            var data = await httpResponse.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<PagedResult<UserViewModel>>(data);

            return new ApiSuccessResult<PagedResult<UserViewModel>>(users);
        }

        public async Task<ApiResult<bool>> RegisterUser(RegisterRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await client.PostAsync("/api/users/", httpContent);
            var result = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest request)
        {
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await client.PutAsync($"/api/users/{id}", httpContent);

            var result = await httpResponse.Content.ReadAsStringAsync();
            if (!httpResponse.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);

            return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
        }
    }
}
using eTweb.ViewModels.Common;
using eTweb.ViewModels.System.Users;
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

        public UserApiClient(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<string> Authenticate(LoginRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await client.PostAsync("/api/users/authenticate", httpContent);

            var tokens = await httpResponse.Content.ReadAsStringAsync();

            return tokens;
        }

        public async Task<PagedResult<UserViewModel>> GetUsersPaging(GetUsersRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.BearerToken);

            var httpResponse = await client.GetAsync(
                    $"/api/users/paging?" +
                    $"pageIndex={request.PageIndex}&" +
                    $"pageSize={request.PageSize}&" +
                    $"keyword={request.Keyword}");

            var data = await httpResponse.Content.ReadAsStringAsync();

            var users = JsonConvert.DeserializeObject<PagedResult<UserViewModel>>(data);

            return users;
        }
    }
}
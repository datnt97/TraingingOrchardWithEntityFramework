using eTweb.ViewModels.Common;
using eTweb.ViewModels.System.Roles;
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
    public class UserApiClient : BaseApiClient, IUserApiClient
    {
        public UserApiClient(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
            : base(
                  httpClientFactory,
                  httpContextAccessor,
                  configuration)
        {
        }

        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            return await PostAsync<ApiResult<string>, LoginRequest>("/api/users/authenticate", request);
        }

        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            return await DeleteAsync<ApiResult<bool>>($"/api/users/{id}");
        }

        public async Task<ApiResult<UserViewModel>> GetById(Guid id)
        {
            return await GetAsync<ApiResult<UserViewModel>>($"/api/users/{id}");
        }

        public async Task<ApiResult<PagedResult<UserViewModel>>> GetUsersPaging(GetUsersRequest request)
        {
            return await GetAsync<ApiResult<PagedResult<UserViewModel>>>(
                    $"/api/users/paging?" +
                    $"pageIndex={request.PageIndex}&" +
                    $"pageSize={request.PageSize}&" +
                    $"keyword={request.Keyword}");
        }

        public async Task<ApiResult<bool>> RegisterUser(RegisterRequest request)
        {
            return await PostAsync<ApiResult<bool>, RegisterRequest>("/api/users/", request);
        }

        public async Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest request)
        {
            return await PutAsync<ApiResult<bool>, UserUpdateRequest>($"/api/users/{id}", request);
        }

        public async Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request)
        {
            return await PutAsync<ApiResult<bool>, RoleAssignRequest>($"/api/users/{request.Id}/roles/", request);
        }
    }
}
using eTweb.ViewModels.Common;
using eTweb.ViewModels.System.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eTweb.AdminApp.Services
{
    public class RoleApiClient : BaseApiClient, IRoleApiClient
    {
        public RoleApiClient(
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(
                  httpClientFactory,
                  httpContextAccessor,
                  configuration)
        { }

        public async Task<ApiResult<List<RoleViewModel>>> GetAll()
        {
            return await GetAsync<ApiResult<List<RoleViewModel>>>("/api/roles/");
        }
    }
}
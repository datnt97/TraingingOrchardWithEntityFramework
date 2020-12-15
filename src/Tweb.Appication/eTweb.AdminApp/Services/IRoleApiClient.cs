using eTweb.ViewModels.Common;
using eTweb.ViewModels.System.Roles;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eTweb.AdminApp.Services
{
    public interface IRoleApiClient
    {
        Task<ApiResult<List<RoleViewModel>>> GetAll();
    }
}
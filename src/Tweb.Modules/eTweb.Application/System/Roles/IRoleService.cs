using eTweb.ViewModels.Common;
using eTweb.ViewModels.System.Roles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eTweb.Application.System.Roles
{
    public interface IRoleService
    {
        Task<ApiResult<List<RoleViewModel>>> GetAll();
    }
}
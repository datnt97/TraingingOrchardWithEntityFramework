using eTweb.ViewModels.Common;
using eTweb.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eTweb.Application.System
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authencate(LoginRequest request);

        Task<ApiResult<bool>> Register(RegisterRequest request);

        Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request);

        Task<ApiResult<PagedResult<UserViewModel>>> GetUsersPaging(GetUsersRequest request);

        Task<ApiResult<UserViewModel>> GetUserById(Guid id);

        Task<ApiResult<bool>> Delete(Guid Id);
    }
}
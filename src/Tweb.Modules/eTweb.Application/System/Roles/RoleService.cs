using eTweb.Data.Entities;
using eTweb.ViewModels.Common;
using eTweb.ViewModels.System.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTweb.Application.System.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ApiResult<List<RoleViewModel>>> GetAll()
        {
            var roles = await _roleManager.Roles.Select(x => new RoleViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToListAsync();

            if (roles.Count() == 0)
            {
                return new ApiErrorResult<List<RoleViewModel>>("Không tìm thấy roles.");
            }

            return new ApiSuccessResult<List<RoleViewModel>>(roles);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTweb.Application.System;
using eTweb.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eTweb.BackenApi.Controllers
{
    /// <summary>
    /// Contains all methods performance authentication.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Authencate(request);
            if (string.IsNullOrEmpty(result.ResultObj))
                return BadRequest("Username or password incorrect.");

            return Ok(result.ResultObj);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Register(request);
            if (!result.IsSuccessed)
                return BadRequest("Register is unsuccessful.");

            return Ok();
        }

        /// <summary>
        /// localhost:port/api/paging?pageIndex=1&pageSize=10&Keyword=
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("paging")]
        public async Task<IActionResult> GetUsersPaging([FromQuery] GetUsersRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var users = await _userService.GetUsersPaging(request);

            return Ok(users);
        }

        /// <summary>
        /// Put: localhost:port/api/users/id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Register(Guid id, [FromBody] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Update(id, request);
            if (!result.IsSuccessed)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Get: localhost:port/api/users/id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var result = await _userService.GetUserById(id);

            return Ok(result);
        }
    }
}
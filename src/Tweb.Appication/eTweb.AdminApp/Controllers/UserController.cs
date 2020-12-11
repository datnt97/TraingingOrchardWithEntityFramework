using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTweb.AdminApp.Services;
using eTweb.ViewModels.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTweb.AdminApp.Controllers
{
    /// <summary>
    /// localhost:port/user/user/
    /// </summary>
    //[Route("user/[controller]")]
    //[AllowAnonymous]
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;

        public UserController(
            IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tokens = await _userApiClient.Authenticate(request);

            return Ok(tokens);
        }
    }
}
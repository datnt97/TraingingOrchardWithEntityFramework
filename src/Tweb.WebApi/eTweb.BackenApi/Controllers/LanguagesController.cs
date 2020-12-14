using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTweb.Application.System.Languages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eTweb.BackenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LanguagesController : ControllerBase
    {
        private readonly ILanguageService _languageService;

        public LanguagesController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _languageService.GetAll();

            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
    }
}
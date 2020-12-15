using eTweb.AdminApp.Models;
using eTweb.AdminApp.Services;
using eTweb.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTweb.AdminApp.Controllers.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly ILanguageApiClient _languageApiClient;

        public NavigationViewComponent(
            ILanguageApiClient languageApiClient)
        {
            _languageApiClient = languageApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _languageApiClient.GetAll();

            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }

            var navigationViewModel = new NavigationViewModel()
            {
                CurrentLanguageId = HttpContext
                    .Session
                    .GetString(SystemConstant.AppSettings.DefaultLanguageId),
                Languages = result.ResultObj
            };

            return View("Default", navigationViewModel);
        }
    }
}
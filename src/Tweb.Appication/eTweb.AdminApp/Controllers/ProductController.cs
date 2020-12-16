using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eTweb.AdminApp.Services;
using eTweb.Utilities.Constants;
using eTweb.ViewModels.Catalog.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace eTweb.AdminApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductController(
            IProductApiClient productApiClient,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _productApiClient = productApiClient;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 1)
        {
            var languageId = _httpContextAccessor.HttpContext.Session.GetString(SystemConstant.AppSettings.DefaultLanguageId);
            var request = new GetManageProductPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                LanguageId = languageId
            };

            var data = await _productApiClient.GetAllPaging(request);
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }

            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _productApiClient.Create(request);

            if (result < 1)
            {
                ModelState.AddModelError("", "Tạo sản phẩm không thành công.");
            }

            TempData["result"] = "Tạo mới sản phẩm thành công.";
            return RedirectToAction("Index");
        }
    }
}
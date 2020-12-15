using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using eTweb.BackenApi.Models;
using eTweb.Application.Catalog.Products;
using eTweb.ViewModels.Catalog.Products;
using eTweb.ViewModels.Catalog.ProductImages;
using Microsoft.AspNetCore.Authorization;

namespace eTweb.BackenApi.Controllers
{
    /// <summary>
    /// api/products
    /// Contains all methods for products
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(
            IProductService manageProductService)
        {
            _productService = manageProductService;
        }

        #region Products

        //// localhost:port/products
        //[HttpGet("{languageId}")]
        //public async Task<ActionResult> Get(string languageId)
        //{
        //    var products = await _publicProductService.GetAll(languageId);
        //    return Ok(products);
        //}

        /// <summary>
        /// Get all products on Page
        /// Ex: localhost:port/products?pageIndex=1&pageSize=10&CategoryId=
        /// </summary>
        /// <param name="languageId">String language Id</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("{languageId}")]
        public async Task<IActionResult> GetAllPaging(string languageId, [FromQuery] GetPublicProductPagingRequest request)
        {
            var products = await _productService.GetAllByCategoryId(languageId, request);

            return Ok(products);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetManageProductPagingRequest request)
        {
            var products = await _productService.GetAllPaging(request);
            return Ok(products);
        }

        // localhost:port/products/1
        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _productService.GetById(productId, languageId);

            if (product == null)
                return BadRequest();

            return Ok(product.Id);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productId = await _productService.Create(request);

            if (productId == 0)
                return BadRequest();

            var product = await _productService.GetById(productId, request.LanguageId);

            return CreatedAtAction(nameof(GetById), new { id = productId }, product);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            var affectedResult = await _productService.Update(request);
            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var affectedResult = await _productService.Delete(productId);
            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

        [HttpPatch("price/{productId}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isSuccessful = await _productService.UpdatePrice(productId, newPrice);
            if (!isSuccessful)
                return BadRequest();

            return Ok();
        }

        #endregion Products

        #region Images

        /// <summary>
        /// Gets Image by Id
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns>
        /// The ProductImageViewModel
        /// </returns>
        [HttpGet("{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var image = await _productService.GetImageById(imageId);
            if (image == null)
                return BadRequest();

            return Ok(image);
        }

        /// <summary>
        /// Creates an image of the product.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{productId}/images")]
        public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imageId = await _productService.AddImage(productId, request);

            var image = await _productService.GetImageById(imageId);
            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }

        /// <summary>
        /// Updates image of the product by id.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{productId}/images")]
        public async Task<IActionResult> UpdateImage(int productId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _productService.UpdateImage(productId, request.Id, request);

            if (result == 0)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> RemoveImage(int productId, int imageId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var affectedResult = await _productService.RemoveImage(productId, imageId);

            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

        #endregion Images
    }
}
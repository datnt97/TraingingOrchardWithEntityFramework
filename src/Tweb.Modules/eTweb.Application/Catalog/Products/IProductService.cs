using eTweb.ViewModels.Catalog.ProductImages;
using eTweb.ViewModels.Catalog.Products;
using eTweb.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eTweb.Application.Catalog.Products
{
    public interface IProductService
    {
        Task<int> Create(ProductCreateRequest request);

        Task<int> Update(ProductUpdateRequest request);

        Task<bool> UpdatePrice(int productId, decimal newPrice);

        Task<bool> UpdateStock(int productId, int addedQuantity);

        Task AddViewcount(int productId);

        Task<int> Delete(int productId);

        Task<ProductViewModel> GetById(int productId, string languageId);

        Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request);

        Task<int> AddImage(int productId, ProductImageCreateRequest request);

        /// <summary>
        /// Updates the image of this product
        /// </summary>
        /// <param name="productId">The product id.</param>
        /// <param name="imageId">The image id</param>
        /// <param name="request">Image model that to update this image.</param>
        /// <returns>
        /// The number of images has been updated.
        /// </returns>
        Task<int> UpdateImage(int productId, int imageId, ProductImageUpdateRequest request);

        Task<int> RemoveImage(int productId, int imageId);

        Task<ProductImageViewModel> GetImageById(int imageId);

        Task<List<ProductImageViewModel>> GetListImages(int productId);

        Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request);
    }
}
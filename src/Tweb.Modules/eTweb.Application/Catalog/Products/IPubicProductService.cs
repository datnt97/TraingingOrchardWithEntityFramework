
using eTweb.Application.Dtos;
using eTweb.ViewModels.Catalog.Products;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eTweb.Application.Catalog.Products
{
    public interface IPubicProductService
    {
        /// <summary>
        /// Get products
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request);
        //Task<List<ProductViewModel>> GetAll(string languageId);
    }
}

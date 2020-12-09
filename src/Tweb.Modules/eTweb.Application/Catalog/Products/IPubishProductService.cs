
using eTweb.Application.Dtos;
using eTweb.ViewModels.Catalog.Products;
using eTweb.ViewModels.Catalog.Products.Public;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eTweb.Application.Catalog.Products
{
    public interface IPubishProductService
    {
        Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request);
    }
}

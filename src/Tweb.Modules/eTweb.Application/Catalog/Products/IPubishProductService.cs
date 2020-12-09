using eTweb.Application.Catalog.Dtos;
using eTweb.Application.Catalog.Dtos.Public;
using eTweb.Application.Dtos;
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

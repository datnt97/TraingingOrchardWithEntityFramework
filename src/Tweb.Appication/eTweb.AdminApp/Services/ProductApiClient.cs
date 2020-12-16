using eTweb.ViewModels.Catalog.Products;
using eTweb.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace eTweb.AdminApp.Services
{
    public class ProductApiClient : BaseApiClient, IProductApiClient
    {
        public ProductApiClient(
            IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(
                  httpClientFactory,
                  httpContextAccessor,
                  configuration)
        {
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var result = await base.PostAsync<int, ProductCreateRequest>($"/api/products/", request);
            return result;
        }

        public Task<int> Delete(ProductUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            var data = await GetAsync<PagedResult<ProductViewModel>>(
                $"/api/products/paging?languageId={request.LanguageId}&" +
                $"pageIndex={request.PageIndex}&" +
                $"pageSize={request.PageSize}&" +
                $"keyword={request.Keyword}");

            return data;
        }

        public Task<int> Update(ProductUpdateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
using eTweb.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace eTweb.ViewModels.Catalog.Products
{
    public class GetPublicProductPagingRequest : PagingRequestBase
    {
        public string LanguageId { get; set; }
        public int? CategoryId { get; set; }
    }
}

using eTweb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eTweb.ViewModels.Catalog.Products
{
    public class GetProductsRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}

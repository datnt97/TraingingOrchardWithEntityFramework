using eTweb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eTweb.ViewModels.Catalog.Products
{
    public class GetManageProductPagingRequest : PagingRequestBase
    {
        public List<int> CategoryIds { get; set; }
        public string Keyword { get; set; }

    }
}

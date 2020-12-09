using eTweb.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace eTweb.Application.Catalog.Dtos.Manage
{
    public class GetManageProductPagingRequest : PagingRequestBase
    {
        public List<int> CategoryIds { get; set; }
        public string Keyword { get; set; }

    }
}

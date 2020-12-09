using eTweb.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace eTweb.Application.Catalog.Dtos.Public
{
    public class GetPublicProductPagingRequest : PagingRequestBase
    {
        public int? CategoryId { get; set; }
    }
}

﻿using eTweb.Application.Dtos;
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

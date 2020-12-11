using eTweb.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eTweb.ViewModels.System.Users
{
    public class GetUsersRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
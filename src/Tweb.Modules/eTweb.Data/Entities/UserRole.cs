﻿using System;
using System.Collections.Generic;
using System.Text;

namespace eTweb.Data.Entities
{
    public class AppRole : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;

#nullable disable

namespace MmtShopApi.Models
{
    public partial class Product
    {
        public int Sku { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}

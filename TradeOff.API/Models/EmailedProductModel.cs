﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeOff.API.Models
{
    public class EmailedProductModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeOff.API.Models
{
    public class ProductImageModel
    {
        [Required]
        public byte[] Image { get; set; }
    }
}

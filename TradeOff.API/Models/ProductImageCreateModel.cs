using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeOff.API.Models
{
    public class ProductImageCreateModel
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public byte[] Image { get; set; }
        [Required]
        public bool IsMainImage { get; set; }
    }
}

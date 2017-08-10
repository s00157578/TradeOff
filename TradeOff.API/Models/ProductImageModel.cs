using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeOff.API.Models
{
    public class ProductImageModel
    {
        public int Id { get; set; }
        public byte[] Image { get; set; }
        public bool IsMainImage { get; set; }
    }
}

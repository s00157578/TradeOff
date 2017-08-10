using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeOff.API.Models
{
    public class ProductImageUpdateModel
    {
        public int ProductId { get; set; }
        public byte[] Image { get; set; }
        public bool IsMainImage { get; set; }
    }
}

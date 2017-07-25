using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TradeOff.API.Entities
{
    public class ProductImage
    {
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        [Required]
        public byte[] Image { get; set; }
        public Product Product { get; set; }
    }
}

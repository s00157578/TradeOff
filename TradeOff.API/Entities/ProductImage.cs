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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        [Required]
        public byte[] Image { get; set; }
        [Required]
        public bool IsMainImage { get; set; }
        public Product Product { get; set; }
    }
}

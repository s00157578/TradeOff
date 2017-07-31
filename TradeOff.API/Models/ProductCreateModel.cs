using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeOff.API.Models
{
    public class ProductCreateModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        [MaxLength(140)]
        [Required]
        public string ShortDescription { get; set; }
        [MaxLength(500)]
        public string FullDescription { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}

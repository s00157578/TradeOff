using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TradeOff.API.Entities;

namespace TradeOff.API.Models
{
    public class ProductModel
    {
        [Required]
        public int Id { get; set; }
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
        public string UserId { get; set; }   
        public int CategoryId { get; set; }

    }
}

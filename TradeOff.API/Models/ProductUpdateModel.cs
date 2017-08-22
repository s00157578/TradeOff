using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeOff.API.Models
{
    public class ProductUpdateModel
    {
        public string Name { get; set; }
        public string Location { get; set; }
        [MaxLength(140)]
        public string ShortDescription { get; set; }
        [MaxLength(500)]
        public string FullDescription { get; set; }
        public decimal Price { get; set; }
        public string UserId { get; set; }
    }
}

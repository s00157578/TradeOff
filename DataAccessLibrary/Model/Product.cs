using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Model
{
    public class Product 
    {
        public int ProductId { get; set; }
        public string ImagePath { get; set; }
        public Coordinates Coordinates { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public decimal Price { get; set; }
    }
}

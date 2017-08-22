using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TradeOff.API.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; } 
        public virtual Category Category { get; set; }
        [Required]
        public string UserId { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    }
}

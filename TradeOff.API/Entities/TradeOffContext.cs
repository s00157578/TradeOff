using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TradeOff.API.Entities
{
    public sealed class TradeOffContext : DbContext
    {
        public TradeOffContext(DbContextOptions<TradeOffContext> options)
            : base(options)
        {
            Database.Migrate();
        }
        public  DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}

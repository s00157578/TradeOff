using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TradeOff.API.Entities
{
    public class User : IdentityUser
    {
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeOff.API.Entities;

namespace TradeOff.API
{
    public class TradeOffIdentitySeeder
    {
        private TradeOffContext _context;

        public TradeOffIdentitySeeder(TradeOffContext context)
        {
            _context = context;
        }
        public void SeedUserIdenities()
        {
            if (!_context.Users.Any())
            {
                var userStore = new UserStore<IdentityUser>(_context);
                var user = new IdentityUser()
                {
                    UserName = "kVesey",
                    Email = "S00157578@mail.itsligo.ie"
                };
                AddIdentity(user, userStore);
                user = new IdentityUser()
                {
                    UserName = "jBlogg",
                    Email = "noMail@mail.itsligo.ie"
                };
                AddIdentity(user, userStore);
                user = new IdentityUser()
                {
                    UserName = "sBlogg",
                    Email = "stillNoMail@mail.itsligo.ie"
                };
                AddIdentity(user, userStore);
                user = new IdentityUser()
                {
                    UserName = "kvesey2",
                    Email = "kevinvesey7@gmail.com"
                };
                AddIdentity(user, userStore);
                _context.SaveChanges();
            }
        }
        private void AddIdentity(IdentityUser user, UserStore<IdentityUser> userStore)
        {
            var password = new PasswordHasher<IdentityUser>();
            var hashed = password.HashPassword(user, "password");
            user.PasswordHash = hashed;
            userStore.CreateAsync(user);            
        }
    }
}

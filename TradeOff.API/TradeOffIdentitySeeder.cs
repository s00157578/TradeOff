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
                var userStore = new UserStore<User>(_context);
                var user = new User()
                {
                    UserName = "kVesey",
                    FirstName = "Kevin",
                    LastName = "Vesey",
                    Email = "S00157578@mail.itsligo.ie"
                };
                addIdentity(user, userStore);
                user = new User()
                {
                    UserName = "jBlogg",
                    FirstName = "Joe",
                    LastName = "Blogg",
                    Email = "noMail@mail.itsligo.ie"
                };
                addIdentity(user, userStore);
                user = new User()
                {
                    UserName = "sBlogg",
                    FirstName = "Steve",
                    LastName = "Blogg",
                    Email = "stillNoMail@mail.itsligo.ie"
                };
                addIdentity(user, userStore);
                user = new User()
                {
                    UserName = "kvesey2",
                    FirstName = "kev",
                    LastName = "vesey",
                    Email = "kevinvesey7@gmail.com"
                };
                addIdentity(user, userStore);
                _context.SaveChanges();
            }
        }
        private void addIdentity(User user, UserStore<User> userStore)
        {
            var password = new PasswordHasher<User>();
            var hashed = password.HashPassword(user, "password");
            user.PasswordHash = hashed;
            userStore.CreateAsync(user);            
        }
    }
}

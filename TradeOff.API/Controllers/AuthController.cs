using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeOff.API.Entities;
using TradeOff.API.Models;

namespace TradeOff.API.Controllers
{
    public class AuthController : Controller
    {
        private TradeOffContext _context;
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        public AuthController(TradeOffContext context, SignInManager<User> signInMgr, UserManager<User> userManager)
        {
            _context = context;
            _signInManager = signInMgr;
            _userManager = userManager;
        }
        [HttpPost("api/auth/login")]
        public async Task<IActionResult> Login([FromBody] CredentialModel model)
        {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
                if (result.Succeeded)
                {
                    return Ok();
                }
            return BadRequest("Failed to login");
        }
        [HttpPost("api/auth/createAccount")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountModel model)
        {
            var userStore = new UserStore<User>(_context);
            var user = new User()
            {
                UserName = model.UserName,
                Email = model.Email
            };
            AddIdentity(user, userStore, model.Password);
            _context.SaveChanges();
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest("Failed to create account ");
        }
        private void AddIdentity(User user, UserStore<User> userStore, string userPassword)
        {
            var password = new PasswordHasher<User>();
            var hashed = password.HashPassword(user, userPassword);
            user.PasswordHash = hashed;
            userStore.CreateAsync(user);
        }
        [HttpPost("api/auth/logOut")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
        [HttpGet("api/auth/UserId")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetUserId()
        {
            var id =  _userManager.GetUserId(User);
            if (id != null)
                return Ok(id);
            return NotFound();
        }
    }
}
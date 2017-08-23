using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeOff.API.Entities;
using TradeOff.API.Models;

namespace TradeOff.API.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JWTSettings _options;
        //secret for coding token
        private byte[] secret = new byte[] { 164, 60, 194, 0, 161, 189, 41, 38, 130, 89, 141, 164, 45, 170, 159, 209, 69, 137, 243, 216, 191, 131, 47, 250, 32, 107, 231, 117, 37, 158, 225, 234 };

        public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IOptions<JWTSettings> optionsAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _options = optionsAccessor.Value;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CredentialModel Credentials)
        {
            if (ModelState.IsValid)
            {
                //creates new identityUser
                var user = new IdentityUser { UserName = Credentials.Email, Email = Credentials.Email };
                var result = await _userManager.CreateAsync(user, Credentials.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return new JsonResult(new Dictionary<string, object>
                    {
                        { "idToken", GetIdToken(user) }
                    });
                }
                return Errors(result);

            }
            return Error("Unexpected error");
        }
        private string GetIdToken(IdentityUser user)
        {
            var payload = new Dictionary<string, object>
      {
        { "id", user.Id },
        { "sub", user.Email },
        { "email", user.Email },
      };
            return GetToken(payload);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CredentialModel Credentials)
        {
            //login  if modelstate is valid
            if (ModelState.IsValid)
            {
                //signs user in
                var result = await _signInManager.PasswordSignInAsync(Credentials.Email, Credentials.Password, false, false);
                //if signed in
                if (result.Succeeded)
                {
                    //gets the user
                    var user = await _userManager.FindByEmailAsync(Credentials.Email);
                    return new JsonResult(new Dictionary<string, object>
                    {
                        //creates a token for the user
                        { "idToken", GetIdToken(user) }
                    });
                }
                return new JsonResult("Unable to sign in") { StatusCode = 401 };
            }
            return Error("Unexpected error");
        }     
        private string GetToken(Dictionary<string, object> payload)
        {
            //adds to token, issuer, the audience, the dateTimes and expiry date in 7 days

            payload.Add("iss", _options.Issuer);
            payload.Add("aud", _options.Audience);
            payload.Add("nbf", ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("iat", ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("exp", ConvertToUnixTimestamp(DateTime.Now.AddDays(7)));
            //coding algorithm
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            //serializer to json
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            //encoder 
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            //encodes and returns the encoded token
            return encoder.Encode(payload, secret);
        }
        //list of errors
        private JsonResult Errors(IdentityResult result)
        {
            var items = result.Errors
                .Select(x => x.Description)
                .ToArray();
            return new JsonResult(items) { StatusCode = 400 };
        }

        //returns the error messafe
        private JsonResult Error(string message)
        {
            return new JsonResult(message) { StatusCode = 400 };
        }
        //creates time stamp
        private static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
        [Authorize]
        [HttpGet("userId")]
        //gets userId
        public async Task<IActionResult> GetUserId()
        {
            var user = await GetCurrentUserAsync();
            var userId = user?.Id;
            if(userId !=null)
                return Ok(userId);
            return NotFound();
        }
        //gets the current user
        private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        [HttpPost("logOut")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
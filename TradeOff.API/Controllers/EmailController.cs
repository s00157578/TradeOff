﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeOff.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using TradeOff.API.Entities;

namespace TradeOff.API.Controllers
{
    [Route("api/email")]
    public class EmailController : Controller
    {
        private IProductRepository _productRepository;
        private IEmailRepository _emailRepository;
        private readonly UserManager<IdentityUser> _userManager;
        public EmailController(IProductRepository productRepository,IEmailRepository emailRepository, UserManager<IdentityUser> userManager)
        {
            _productRepository = productRepository;
            _emailRepository = emailRepository;
            _userManager = userManager;           
        }

        [HttpGet("{productId}/sendEmail")]
        [Authorize]
        public async Task<IActionResult> SendEmail(int productId)
        {          
            var buyingUserId = _userManager.GetUserId(HttpContext.User);
            if (string.IsNullOrEmpty(buyingUserId))
                return BadRequest();

            var product = _productRepository.GetProduct(productId);
            if (product == null)
                return NotFound();

            string buyingEmail = _emailRepository.GetUserEmail(buyingUserId);
            string sellingEmail = _emailRepository.GetUserEmail(product.UserId);
            if (string.IsNullOrEmpty(buyingEmail) || string.IsNullOrEmpty(sellingEmail))
                return NotFound();
            if (_emailRepository.HasEmailedBefore(buyingUserId, product.Id))
                return BadRequest("user already emailed seller of product");
            var apiKey = Environment.GetEnvironmentVariable("SendGridKey");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("TradeOffProject@gmail.com", "TradeOff");            
            var to = new EmailAddress(sellingEmail, sellingEmail);
            var subject = $"Interest in purchase of: {product.Name}";
            var plainTextContent = "";
            var htmlContent = $@"Hi, <a>{buyingEmail}</a> is interested in the product you are selling : {product.Name}. 
Please get in touch with the user at the following email address: <a>{buyingEmail}</a>

if the product is no longer for sale please log in to your account and delete the listing.
Yours,
TradeOff Team.";
            var message = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(message);
            if(response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                EmailedProduct emailedProduct = new EmailedProduct() { UserId = buyingUserId, ProductId = product.Id };
                _emailRepository.AddEmail(emailedProduct);
                return NoContent();              
            }
            return BadRequest();
        }
        [HttpGet("{productId}/hasEmailedBefore")]
        public IActionResult HasEmailedBefore(int productId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
                return BadRequest();
            var product = _productRepository.GetProduct(productId);
            if (product == null)
                return NotFound();

            var hasEmailed = _emailRepository.HasEmailedBefore(userId, product.Id);
            return Ok(hasEmailed);
        }
    }
}

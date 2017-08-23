using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeOff.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using TradeOff.API.Entities;
using TradeOff.API.Models;
using AutoMapper;

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
            //gets the buying users ID (logged in user)
            var buyingUserId = _userManager.GetUserId(HttpContext.User);
            if (string.IsNullOrEmpty(buyingUserId))
                return BadRequest();
            //gets product
            var product = _productRepository.GetProduct(productId);
            if (product == null)
                return NotFound();
            //gets buying and selling email based on userId
            string buyingEmail = _emailRepository.GetUserEmail(buyingUserId);
            string sellingEmail = _emailRepository.GetUserEmail(product.UserId);
            if (string.IsNullOrEmpty(buyingEmail) || string.IsNullOrEmpty(sellingEmail))
                return NotFound();
            //checks if email has been sent before
            if (_emailRepository.HasEmailedBefore(buyingUserId, product.Id))
                return BadRequest("user already emailed seller of product");
            //sendgrid api key
            var sendGridapiKey = "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";
            //creates email, based of sendgrid example
            var client = new SendGridClient(sendGridapiKey);
            var from = new EmailAddress("TradeOffProject@gmail.com", "TradeOff");            
            var to = new EmailAddress(sellingEmail, sellingEmail);
            var subject = $"Interest in purchase of: {product.Name}";
            var plainTextContent = "";
            var htmlContent = $@"Hi,<br/> <a href='mailto:{buyingEmail}?Subject=TradeOff,%20{product.Name}%20for%20sale' target='_top'>{buyingEmail}</a> is interested in the product you are selling: <br/> {product.Name}. 
<br/><br/>Please get in touch with the user at the following email address: <br/><ahref='mailto:{buyingEmail}?Subject=TradeOff,%20{product.Name}%20for%20sale' target='_top'>{buyingEmail}</a>
<br/><br/>
if the product is no longer for sale please log in to your account and delete the listing.
<br/><br/>
Yours,
<br/>
TradeOff Team.";
            //sends email
            var message = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            //checks response and returns (can't just return response because I am using a .net core api
            var response = await client.SendEmailAsync(message);
            if(response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                EmailedProductModel emailedProduct = new EmailedProductModel() { UserId = buyingUserId, ProductId = product.Id };
                var emailedEntity = Mapper.Map<Entities.EmailedProduct>(emailedProduct);
                _emailRepository.AddEmail(emailedEntity);
                return NoContent();              
            }
            return BadRequest("Email could not be sent");
        }
        [HttpGet("{productId}/hasEmailedBefore")]
        //checks if the user has emailed before
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

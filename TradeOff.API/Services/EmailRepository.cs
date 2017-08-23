using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeOff.API.Entities;

namespace TradeOff.API.Services
{
    public class EmailRepository : IEmailRepository
    {
        private TradeOffContext _context;
        public EmailRepository(TradeOffContext context)
        {
            _context = context;
        }

        public void AddEmail(EmailedProduct emailedProduct)
        {
            _context.EmailedProducts.Add(emailedProduct);
            Save();
        }

        public void DeleteEmailedProducts(int productId)
        {
            var emailedProducts = GetEmailedProducts(productId);
            foreach (var emailed in emailedProducts)
            {
                _context.EmailedProducts.Remove(emailed);
            }
        }

        public IEnumerable<EmailedProduct> GetEmailedProducts(int productId)
        {
            return _context.EmailedProducts.Where(x => x.ProductId == productId).ToList();
        }

        public string GetUserEmail(string id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
                return "";
            return user.Email;
        }

        public bool HasEmailedBefore(string userId, int productId)
        {
            var hasEmailed = _context.EmailedProducts.FirstOrDefault(x => x.UserId == userId && x.ProductId == productId);
            if (hasEmailed == null)
                return false;
            return true;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

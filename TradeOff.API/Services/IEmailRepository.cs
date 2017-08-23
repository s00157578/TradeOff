using System.Collections.Generic;
using TradeOff.API.Entities;

namespace TradeOff.API.Services
{
    public interface IEmailRepository
    {
        string GetUserEmail(string id);
        void DeleteEmailedProducts(int productId);
        IEnumerable<EmailedProduct> GetEmailedProducts(int productId);
        void Save();
        bool HasEmailedBefore(string userId, int productId);
        void AddEmail(EmailedProduct emailedProduct);
    }
}

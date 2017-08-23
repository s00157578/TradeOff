using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeOffAndroidApp.Core.Services
{
    public class EmailRepository
    {
        private static APIConnecter _apiConnecter = new APIConnecter();
        public async Task<bool> EmailSeller(int productId)
        {
            string url = UrlResourceName.ResourceName + $"api/email/{productId}/sendEmail";
            string content = await _apiConnecter.GetResponseJsonString(url);
            if (content == "true")
                return true;
            return false;
        }
        public async Task<bool> HasEmailedBefore(int productId)
        {
            string url = UrlResourceName.ResourceName + $"api/email/{productId}/hasEmailedBefore";
            string content = await _apiConnecter.GetResponseJsonString(url);
            if (content == "true")
                return true;
            return false;
        }
    }
}

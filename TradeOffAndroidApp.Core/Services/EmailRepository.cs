using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TradeOffAndroidApp.Core.Services
{
    public class EmailRepository
    {
        private static APIConnecter _apiConnecter = new APIConnecter();
        public bool EmailSeller(int productId)
        {
            string url = UrlResourceName.ResourceName + $"api/email/{productId}/sendEmail";
            var baseAddress = new Uri(url);
            //gets token from secure storage
            var idToken = CrossSecureStorage.Current.GetValue("idToken");
            //for sending cookies
            CookieContainer cookies = new CookieContainer();
            using (var handler = new HttpClientHandler()
            {
                CookieContainer = cookies
            })
            //send request using httpclient 
            using (var httpClient = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                if (!string.IsNullOrEmpty(idToken))
                    cookies.Add(baseAddress, new Cookie(".AspNetCore.Identity.Application", idToken));
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                return response.IsSuccessStatusCode;
            }
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

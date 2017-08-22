using Newtonsoft.Json;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TradeOffAndroidApp.Core.Models;

namespace TradeOffAndroidApp.Core.Services
{
    public class CredentialRepository
    {
        private static APIConnecter _apiConnecter = new APIConnecter();
        public void CreateAccountAsync(CredentialModel account)
        {
            string url = UrlResourceName.ResourceName + $"api/auth/register";
            var jsonProduct = JsonConvert.SerializeObject(account);
            var httpContent = new StringContent(jsonProduct, Encoding.UTF8, "application/json");
            SendPostRequestAsync(url, httpContent);
        }
        public void LoginAsync(CredentialModel account)
        {
            string url = UrlResourceName.ResourceName + "api/auth/login";
            var jsonProduct = JsonConvert.SerializeObject(account);
            var httpContent = new StringContent(jsonProduct, Encoding.UTF8, "application/json");
            SendPostRequestAsync(url, httpContent);

        }
        private void SendPostRequestAsync(string url, HttpContent httpContent)
        {
            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();

            handler.CookieContainer = cookies;
            using (var httpClient = new HttpClient(handler))
            {
                HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;
                Uri uri = new Uri(url);
                IEnumerable<Cookie> responseCookies = cookies.GetCookies(uri).Cast<Cookie>();
                Cookie idCookie = responseCookies.FirstOrDefault(c => c.Name == ".AspNetCore.Identity.Application");
                string cookieContent = idCookie.Value;
                CrossSecureStorage.Current.SetValue("idToken", cookieContent);
            }
        }
        public bool LogOut()
        {
            string url = UrlResourceName.ResourceName + $"api/auth/logOut";

            using (var httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = httpClient.GetAsync(url).Result;
                    return response.IsSuccessStatusCode;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
            public async Task<string> GetUserId()
        {      
            string url = UrlResourceName.ResourceName + "api/auth/userId";
            string responseJsonString = await _apiConnecter.GetResponseJsonString(url);
            return JsonConvert.DeserializeObject<string>(responseJsonString);
        }
    }
}

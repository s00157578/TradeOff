using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TradeOffAndroidApp.Core.Models;

namespace TradeOffAndroidApp.Core.Services
{
    public class CredentialRepository
    {
        private static APIConnecter _apiConnecter = new APIConnecter();
        public bool CreateAccount( CreateAccountModel account)
        {
            string url = UrlResourceName.ResourceName + $"api/auth/createAccount";
            var jsonProduct = JsonConvert.SerializeObject(account);
            var httpContent = new StringContent(jsonProduct, Encoding.UTF8, "application/json");
            return SendPostRequest(url, httpContent);
        }
        public bool Login(CredentialModel account)
        {
            string url = UrlResourceName.ResourceName + "api/auth/login";
            var jsonProduct = JsonConvert.SerializeObject(account);
            var httpContent = new StringContent(jsonProduct, Encoding.UTF8, "application/json");
            return SendPostRequest(url, httpContent);
            
        }
        private bool SendPostRequest(string url, HttpContent httpContent)
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;
                return response.IsSuccessStatusCode;
            }
        }
    }
}

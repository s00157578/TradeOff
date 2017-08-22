using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TradeOffAndroidApp.Core
{
    public class APIConnecter
    {
        public async Task<string> GetResponseJsonString(string url)
        {
            string responseJsonString = null;
            var idToken = CrossSecureStorage.Current.GetValue("idToken");           
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + idToken);
                try
                {
                    HttpResponseMessage response = httpClient.GetAsync(url).Result;
                    responseJsonString = await response.Content.ReadAsStringAsync();
                }
                catch(Exception e)
                {
                    throw e;
                }
               
            }
            return responseJsonString;
        }
        public bool DeleteRequest(string url)
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.DeleteAsync(url).Result;
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<string> PostRequest(string url, HttpContent content)
        {
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.PostAsync(url, content).Result;
                string responseJsonString = await response.Content.ReadAsStringAsync();
                return responseJsonString;
            }
        }
        public bool PUtRequest(string url, HttpContent content)
        {
            //var patch = new HttpMethod("PATCH");
            //var request = new HttpRequestMessage(patch, url)
            //{
            //    Content = content
            //};
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.PutAsync(url,content).Result;
                return response.IsSuccessStatusCode;
            }
        }
    }
}

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
            using (var httpClient = new HttpClient())
            {
                Task<HttpResponseMessage> getResponse = httpClient.GetAsync(url);
                HttpResponseMessage response = await getResponse;
                responseJsonString = await response.Content.ReadAsStringAsync();
            }
            return responseJsonString;
        }
        public async Task<bool> DeleteRequest(string url)
        {
            using (var httpClient = new HttpClient())
            {
                Task<HttpResponseMessage> getResponse = httpClient.DeleteAsync(url);
                HttpResponseMessage response = await getResponse;
                return response.IsSuccessStatusCode;
            }            
        }
        public async Task<string> PostRequest(string url, HttpContent content)
        {
            using (var httpClient = new HttpClient())
            {
                Task<HttpResponseMessage> getResponse = httpClient.PostAsync(url, content);
                HttpResponseMessage response = await getResponse;
                string responseJsonString = await response.Content.ReadAsStringAsync();
                return responseJsonString;
            }
        }
        public async Task<bool> PatchRequest(string url, HttpContent content)
        {
            var patch = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(patch, url)
            {
                Content = content
            };
            using (var httpClient = new HttpClient())
            {
                Task<HttpResponseMessage> getResponse = httpClient.SendAsync(request);
                HttpResponseMessage response = await getResponse;
                return response.IsSuccessStatusCode;
            }
        }
    }
}

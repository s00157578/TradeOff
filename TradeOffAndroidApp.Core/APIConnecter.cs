using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TradeOffAndroidApp.Core
{
    public class APIConnecter
    {
        //for any get requests
        public async Task<string> GetResponseJsonString(string url)
        {
            string responseJsonString = null;
            //adress to send to
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
                cookies.Add(baseAddress, new Cookie(".AspNetCore.Identity.Application", idToken));
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                responseJsonString = await response.Content.ReadAsStringAsync();
            }
            return responseJsonString;
        }
        //delete request
        public bool DeleteRequest(string url)
        {
            var baseAddress = new Uri(url);
            var idToken = CrossSecureStorage.Current.GetValue("idToken");

            CookieContainer cookies = new CookieContainer();
            using (var handler = new HttpClientHandler()
            {
                CookieContainer = cookies
            })
            using (var httpClient = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                cookies.Add(baseAddress, new Cookie(".AspNetCore.Identity.Application", idToken));
                HttpResponseMessage response = httpClient.DeleteAsync(url).Result;
                return response.IsSuccessStatusCode;
            }
        }
        //post request
        public async Task<string> PostRequest(string url, HttpContent content)
        {
            string responseJsonString = null;
            var baseAddress = new Uri(url);
            var idToken = CrossSecureStorage.Current.GetValue("idToken");

            CookieContainer cookies = new CookieContainer();
            using (var handler = new HttpClientHandler()
            {
                CookieContainer = cookies
            })
            using (var httpClient = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                cookies.Add(baseAddress, new Cookie(".AspNetCore.Identity.Application", idToken));
                HttpResponseMessage response = httpClient.PostAsync(url, content).Result;
                responseJsonString = await response.Content.ReadAsStringAsync();
            }
            return responseJsonString;
        }
        //put request
        public bool PUtRequest(string url, HttpContent content)
        {
            var baseAddress = new Uri(url);
            var idToken = CrossSecureStorage.Current.GetValue("idToken");

            CookieContainer cookies = new CookieContainer();
            using (var handler = new HttpClientHandler()
            {
                CookieContainer = cookies
            })
            using (var httpClient = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                cookies.Add(baseAddress, new Cookie(".AspNetCore.Identity.Application", idToken));
                HttpResponseMessage response = httpClient.PutAsync(url, content).Result;
                return response.IsSuccessStatusCode;
            }
        }
    }
}

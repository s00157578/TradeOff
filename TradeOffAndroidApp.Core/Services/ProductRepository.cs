using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TradeOffAndroidApp.Core.Services
{
    public class ProductRepository : IProductRepository
    {
        private APIConnecter _apiConnecter;

        public async Task<ProductModel> AddProduct(int categoryId, ProductCreateModel product)
        {
            string url = UrlResourceName.ResourceName + $"api/products/{categoryId}/product";
            var jsonProduct = JsonConvert.SerializeObject(product);
            var httpContent = new StringContent(jsonProduct, Encoding.UTF8, "application/json");
            string responseJson =  await _apiConnecter.PostRequest(url, httpContent);
            return JsonConvert.DeserializeObject<ProductModel>(responseJson);
        }
        public async Task<bool> DeleteProduct(int productId)
        {
            string url = UrlResourceName.ResourceName + $"api/products/{productId}";
            return await _apiConnecter.DeleteRequest(url);
        }
        public async Task<ProductModel> GetProduct(int productId)
        {
            string url = UrlResourceName.ResourceName + $"api/products/{productId}";
            string responseJsonString = await _apiConnecter.GetResponseJsonString(url);
            return JsonConvert.DeserializeObject<ProductModel>(responseJsonString);
        }

        public async Task<IEnumerable<ProductModel>> GetProductsByCategory(int categoryId)
        {
            string url = UrlResourceName.ResourceName + $"api/products/category/{categoryId}";
            string responseJsonString = await _apiConnecter.GetResponseJsonString(url);
            return JsonConvert.DeserializeObject<List<ProductModel>>(responseJsonString).ToList();
        }

        public async Task<bool> UpdateProduct(int categoryId, int productId, ProductUpdateModel product)
        {
            string url = UrlResourceName.ResourceName + $"api/products/{categoryId}/product/{productId}";
            var jsonProduct = JsonConvert.SerializeObject(product);
            var httpContent = new StringContent(jsonProduct, Encoding.UTF8, "application/json");
            return await _apiConnecter.PatchRequest(url, httpContent);
        }
    }
}

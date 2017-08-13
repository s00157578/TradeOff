using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TradeOffAndroidApp.Core.Services
{
    public class ProductImageRepository : IProductImageRepository
    {
        private APIConnecter _apiConnecter = new APIConnecter();
        public async Task<ProductImageModel> AddProductImage(int productId, ProductImageCreateModel productImage)
        {
            string url = UrlResourceName.ResourceName + $"api/products/{productId}/productImage";
            var jsonProduct = JsonConvert.SerializeObject(productImage);
            var httpContent = new StringContent(jsonProduct, Encoding.UTF8, "application/json");
            string responseJson = await _apiConnecter.PostRequest(url, httpContent);
            return JsonConvert.DeserializeObject<ProductImageModel>(responseJson);
        }
        public bool DeleteProductImage(int productImageId)
        {
            string url = UrlResourceName.ResourceName + $"api/products/productImage/{productImageId}";
            return _apiConnecter.DeleteRequest(url);
        }
        public async Task<IEnumerable<ProductImageModel>> GetMainProductImages()
        {
            string url = UrlResourceName.ResourceName + $"api/products/productImage/mainImages";
            string responseJsonString = await _apiConnecter.GetResponseJsonString(url);
            return JsonConvert.DeserializeObject<List<ProductImageModel>>(responseJsonString).ToList(); 
        }

        public async Task<IEnumerable<ProductImageModel>> GetProductImages(int productId)
        {
            string url = UrlResourceName.ResourceName + $"api/products/productImage/{productId}";
            string responseJsonString = await _apiConnecter.GetResponseJsonString(url);
            return JsonConvert.DeserializeObject<List<ProductImageModel>>(responseJsonString).ToList();
        }
        public async Task<IEnumerable<ProductImageModel>> GetMainProductImagesByCategory(int categoryId)
        {
            string url = UrlResourceName.ResourceName + $"api/products/getMainImagesByCategory/{categoryId}";
            string responseJsonString = await _apiConnecter.GetResponseJsonString(url);
            return JsonConvert.DeserializeObject<List<ProductImageModel>>(responseJsonString).ToList();
        }
        public bool UpdateProductImage(int productId, int productImageId, ProductImageUpdateModel productImage)
        {
            string url = UrlResourceName.ResourceName + $"api/products/{productId}/productImage/{productImageId}";
            var jsonProduct = JsonConvert.SerializeObject(productImage);
            var httpContent = new StringContent(jsonProduct, Encoding.UTF8, "application/json");
            return _apiConnecter.PatchRequest(url, httpContent);
        }
    }
}
 
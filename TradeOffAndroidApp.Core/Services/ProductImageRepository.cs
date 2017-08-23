using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TradeOffAndroidApp.Core.Services
{
    public class ProductImageRepository
    {
        //repository pattern for crud product actions, requests are sent from the APIConnecter
        private APIConnecter _apiConnecter = new APIConnecter();
        public async Task<ProductImageModel> AddProductImage(int productId, ProductImageCreateModel productImage)
        {
            string url = UrlResourceName.ResourceName + $"api/products/{productId}/productImage";
            //serializes the object to json
            var jsonProduct = JsonConvert.SerializeObject(productImage);
            ///Sets the http content
            var httpContent = new StringContent(jsonProduct, Encoding.UTF8, "application/json");
            //calls the apiConnecter classes postRequest and passes in the url and conent
            string responseJson = await _apiConnecter.PostRequest(url, httpContent);
            //deserialzes what has been returned
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
            string url = UrlResourceName.ResourceName + $"api/products/{productId}/productImages";
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
            return _apiConnecter.PUtRequest(url, httpContent);
        }
    }
}
 
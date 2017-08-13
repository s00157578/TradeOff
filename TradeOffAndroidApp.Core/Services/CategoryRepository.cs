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
    public class CategoryRepository
    {
        private static APIConnecter _apiConnecter = new APIConnecter();
        public async Task<IEnumerable<CategoryModel>> GetCategoriesAsync()
        {
            string getCategoriesUrl = UrlResourceName.ResourceName + "api/products/categories";
            string responseJsonString = await _apiConnecter.GetResponseJsonString(getCategoriesUrl);
            return JsonConvert.DeserializeObject<List<CategoryModel>>(responseJsonString).ToList();
        }
    }
}
 
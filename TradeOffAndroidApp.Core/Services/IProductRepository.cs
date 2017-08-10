using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeOffAndroidApp.Core.Services
{
    public interface IProductRepository
    {
        Task <IEnumerable<ProductModel>> GetProductsByCategory(int categoryId);
        Task <ProductModel> GetProduct(int productId);
        Task<ProductModel> AddProduct(int categoryId, ProductCreateModel product);
        Task<bool> UpdateProduct(int categoryId, int productId, ProductUpdateModel product);
        Task<bool> DeleteProduct(int productId);
    }
}

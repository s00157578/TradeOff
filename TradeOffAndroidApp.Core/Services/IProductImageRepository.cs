using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeOffAndroidApp.Core.Services
{
    public interface IProductImageRepository
    {
        Task<IEnumerable<ProductImageModel>> GetMainProductImages();
        Task<IEnumerable<ProductImageModel>> GetProductImages(int productId);
        Task<bool> DeleteProductImage(int productImageId);
        Task<ProductImageModel> AddProductImage(int productId, ProductImageCreateModel productImage);
        Task<bool> UpdateProductImage(int productId, int productImageId, ProductImageUpdateModel productImage);
    }
}

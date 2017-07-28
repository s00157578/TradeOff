using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeOff.API.Entities;

namespace TradeOff.API.Services
{
    public interface IProductImageRepository
    {
        bool ProductImageExists(int productImageId);
        void DeleteImagesForProduct(int productId);
        void DeleteProductImage(ProductImage productImage);
        IEnumerable<ProductImage> GetProductImages(int productId);
        bool Save();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeOff.API.Entities;

namespace TradeOff.API.Services
{
    public interface IProductRepository
    {
        bool ProductExists(int productId);
        Category GetCategory(int categoryId);
        IEnumerable<Category> GetCategories();
        IEnumerable<Product> GetProducts();
        Product GetProduct(int productId);
        IEnumerable<Product> GetProductsByCategory(int categoryId);
        void AddProduct(int categoryId, Product product);
        void AddProductImage(int productId, ProductImage image);
        void DeleteProduct(Product product);        
        void DeleteImagesForProduct(int productId);
        void DeleteProductImage(ProductImage productImage);
        IEnumerable<ProductImage> GetProductImages(int productId);
        ProductImage GetProductImage(int productImageId);
        ProductImage GetMainImage(int productId);
        IEnumerable<ProductImage> GetMainProductImages();
        void setMainImageToFalse(int productId, int productImageId);
        bool ProductImageExistsByImageId(int productImageId);
        bool ProductImageExistsByProductId(int productId);
        bool Save();
    }
}

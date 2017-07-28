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
        IEnumerable<Product> GetProducts();
        Product GetProduct(int productId);
        IEnumerable<Product> GetProductsByCategory(int categoryId);
        void AddProduct(int categoryId, Product product);
        void DeleteProduct(Product product);       
        bool Save();
    }
}

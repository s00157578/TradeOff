using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeOff.API.Entities;

namespace TradeOff.API.Services
{
    public class ProductRepository : IProductRepository
    {
        private TradeOffContext _context;
        public void AddProduct(int categoryId, Product product)
        {
            _context.Products.Add(product);
        }
        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
        }
        public Product GetProduct(int productId)
        {
            return _context.Products.FirstOrDefault(p => p.Id == productId);
        }
        public IEnumerable<Product> GetProducts()
        {
            return _context.Products.OrderBy(p => p.Name).ToList();
        }

        public IEnumerable<Product> GetProductsByCategory(int categoryId)
        {
            return _context.Products.OrderBy(p => p.Name).Where(p => p.CategoryId == categoryId).ToList();
        }

        public bool ProductExists(int productId)
        {
            return _context.Products.Any(p => p.Id == productId);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}

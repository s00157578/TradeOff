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
        public ProductRepository(TradeOffContext context)
        {
            _context = context;
        }
        public void AddProduct(int categoryId, Product product)
        {
            var category = GetCategory(categoryId);
            category.Products.Add(product);
        }
        public void AddProductImage(int productId, ProductImage image)
        {
            var product = GetProduct(productId);
            product.ProductImages.Add(image);
        }
        public void DeleteImagesForProduct(int productId)
        {
            var productImages = GetProductImages(productId);
            foreach (var image in productImages)
            {
                _context.ProductImages.Remove(image);
            }
        }

        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
        }

        public void DeleteProductImage(ProductImage productImage)
        {
            
            _context.ProductImages.Remove(productImage);
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.OrderBy(p => p.CategoryName).ToList();
        }

        public Category GetCategory(int categoryId)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == categoryId);
        }

        public ProductImage GetMainImage(int productId)
        {
            return _context.ProductImages.FirstOrDefault(p => p.ProductId == productId && p.IsMainImage);
        }

        public IEnumerable<ProductImage> GetMainImagesByCategory(int categoryId)
        {
            var imageByCategory = from p in _context.Products
                                  join pi in _context.ProductImages on p.Id equals pi.ProductId
                                  where p.CategoryId == categoryId && pi.IsMainImage == true
                                  select pi;
            return imageByCategory;
        }

        public IEnumerable<ProductImage> GetMainProductImages()
        {
            return _context.ProductImages.Where(p => p.IsMainImage == true).ToList();
        }

        public Product GetProduct(int productId)
        {
            return _context.Products.FirstOrDefault(p => p.Id == productId);
        }

        public ProductImage GetProductImage(int productImageId)
        {
            return _context.ProductImages.FirstOrDefault(p => p.Id == productImageId);
        }

        public IEnumerable<ProductImage> GetProductImages(int productId)
        {
            return _context.ProductImages.Where(p => p.ProductId == productId && p.IsMainImage == true).ToList();
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Products.OrderBy(p => p.Name).ToList();
        }

        public IEnumerable<Product> GetProductsByCategory(int categoryId)
        {
            return _context.Products.OrderBy(p => p.Name).Where(p => p.CategoryId == categoryId).ToList();
        }

        public IEnumerable<Product> GetUserProducts(string userId)
        {
            return _context.Products.Where(p => p.UserId == userId).ToList();
        }

        public bool ProductExists(int productId)
        {
            return _context.Products.Any(p => p.Id == productId);
        }

        public bool ProductImageExistsByImageId(int productImageId)
        {
            return _context.ProductImages.Any(p => p.Id == productImageId);
        }
        public bool ProductImageExistsByProductId(int productId)
        {
            return _context.ProductImages.Any(p => p.ProductId == productId);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void setMainImageToFalse(int productId, int productImageId)
        {
            var productImages = GetProductImages(productId);
            foreach (var image in productImages)
            {
                if (image.Id == productImageId)
                    continue;
                image.IsMainImage = false;
                _context.ProductImages.Update(image);
            }
        }
    }
}

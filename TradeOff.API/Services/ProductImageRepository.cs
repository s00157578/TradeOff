using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeOff.API.Entities;

namespace TradeOff.API.Services
{
    public class ProductImageRepository : IProductImageRepository
    {
        private TradeOffContext _context;
        public void DeleteImagesForProduct(int productId)
        {
            IEnumerable<ProductImage> productImages = GetProductImages(productId);
            foreach (var image in productImages)
            {
                _context.ProductImages.Remove(image);
            }
        }
        public void DeleteProductImage(ProductImage productImage)
        {
            _context.ProductImages.Remove(productImage);
        }
        public IEnumerable<ProductImage> GetProductImages(int productId)
        {
            return _context.ProductImages.Where(p => p.ProductId == productId).ToList();
        }

        public bool ProductImageExists(int productImageId)
        {
            return _context.ProductImages.Any(p => p.Id == productImageId);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TradeOff.API.Models;
using TradeOff.API.Services;

namespace TradeOff.API.Controllers
{
    [Route("api/products")]
    public class ProductsController : Controller
    {
        private IProductRepository _productRepository;
        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var productEntities = _productRepository.GetProducts();
            var results = Mapper.Map<IEnumerable<ProductModel>>(productEntities);
            return Ok(results);
        }
        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            var CategoryEntities = _productRepository.GetCategories();
            var results = Mapper.Map<IEnumerable<CategoryModel>>(CategoryEntities);
            return Ok(results);
        }
        [HttpGet("{productId}/productImages")]
        public IActionResult GetProductImages(int productId)
        {
            if (!_productRepository.ProductImageExistsByProductId(productId))
                return NotFound();
            var imageEntities = _productRepository.GetProductImages(productId);
            if (imageEntities == null)
                return NotFound();
            var imageResult = Mapper.Map<IEnumerable<ProductImageModel>>(imageEntities);
            return Ok(imageResult);
        }
        [HttpGet("productimage/{id}")]
        public IActionResult GetProductImage(int id)
        {
            if (!_productRepository.ProductImageExistsByImageId(id))
                return NotFound();
            var image = _productRepository.GetProductImage(id);
            if (image == null)
                return NotFound();
            var imageResult = Mapper.Map<ProductImageModel>(image);
            return Ok(imageResult);
        }
        [HttpGet("productImage/mainImages")]
        public IActionResult GetMainProductImages()
        {
            var imageEntities = _productRepository.GetMainProductImages();
            if (imageEntities == null)
                return NotFound();
            var imageResult = Mapper.Map<IEnumerable<ProductImageModel>>(imageEntities);
            return Ok(imageResult);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _productRepository.GetProduct(id);
            if (product == null)
                return NotFound();
            var productResult = Mapper.Map<ProductModel>(product);
            return Ok(productResult);
        }

        [HttpGet("category/{id}")]
        public IActionResult GetProductsByCategory(int id)
        {
            var products = _productRepository.GetProductsByCategory(id);
            if (products == null)
                return NotFound();
            var productsResult = Mapper.Map<IEnumerable<ProductModel>>(products);
            return Ok(productsResult);
        }

        [HttpPost("{categoryId}/product", Name = "GetProduct")]
        public IActionResult CreateProduct(int categoryId, [FromBody] ProductCreateModel product)
        {
            if (product == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var finalProduct = Mapper.Map<Entities.Product>(product);
            _productRepository.AddProduct(categoryId, finalProduct);
            if (!_productRepository.Save())
                return StatusCode(500, "A problem happened while handling your request");
            var createdProduct = Mapper.Map<Models.ProductModel>(finalProduct);
            return CreatedAtRoute("GetProduct", new { id = createdProduct.Id }, createdProduct);
        }
        [HttpPost("{productId}/productImage", Name = "GetProductImage")]
        public IActionResult CreateProductImage(int productId, [FromBody] ProductImageCreateModel productImage)
        {
            if (productImage == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var finalImage = Mapper.Map<Entities.ProductImage>(productImage);
            _productRepository.AddProductImage(productId, finalImage);
            if (!_productRepository.Save())
                return StatusCode(500, "A problem happened while handling your request");
            var createdImage = Mapper.Map<Models.ProductImageModel>(finalImage);
            return CreatedAtRoute("GetProductImage", new { id = createdImage.Id }, createdImage);
        }

        [HttpPatch("{categoryId}/product/{id}")]
        public IActionResult UpdateProduct(int id,
            [FromBody] JsonPatchDocument<ProductUpdateModel> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }
            if (!_productRepository.ProductExists(id))
            {
                return NotFound();
            }
            var productEntity = _productRepository.GetProduct(id);
            if (productEntity == null)
            {
                return NotFound();
            }
            var productToPatch = Mapper.Map<ProductUpdateModel>(productEntity);
            patchDoc.ApplyTo(productToPatch, ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            TryValidateModel(productToPatch);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Mapper.Map(productToPatch, productEntity);
            if (!_productRepository.Save())
                return StatusCode(500, "A problem happened while handling your request");
            return NoContent();
        }
        [HttpPatch("{productId}/productImage/{id}")]
        public IActionResult UpdateProductImage(int id,
            [FromBody] JsonPatchDocument<ProductImageUpdateModel> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }
            if (!_productRepository.ProductImageExistsByImageId(id))
            {
                return NotFound();
            }
            var imageEntity = _productRepository.GetProductImage(id);
            if (imageEntity == null)
            {
                return NotFound();
            }
            var imageToPatch = Mapper.Map<ProductImageUpdateModel>(imageEntity);
            patchDoc.ApplyTo(imageToPatch, ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if(imageToPatch.IsMainImage == true)
            {
                _productRepository.setMainImageToFalse(imageToPatch.ProductId, id);
            }
            Mapper.Map(imageToPatch, imageEntity);
            if (!_productRepository.Save())
                return StatusCode(500, "A problem happened while handling your request");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            if (!_productRepository.ProductExists(id))
                return NotFound();
            var productEntity = _productRepository.GetProduct(id);
            if (productEntity == null)
                return NotFound();
            _productRepository.DeleteProduct(productEntity);
            _productRepository.DeleteImagesForProduct(id);
            if (!_productRepository.Save())
                return StatusCode(500, "A problem happened while handling your request");
            return NoContent();
        }
        [HttpDelete("productImage/{id}")]
        public IActionResult DeleteProductImage(int id)
        {
            if (!_productRepository.ProductImageExistsByImageId(id))
                return NotFound();
            var imageEntity = _productRepository.GetProductImage(id);
            if (imageEntity == null)
                return NotFound();
            _productRepository.DeleteProductImage(imageEntity);
            if (!_productRepository.Save())
                return StatusCode(500, "A problem happened while handling your request");
            return NoContent();
        }

    }
}
  
using System;
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
            var productsResult = Mapper.Map<ProductModel>(products);
            return Ok(productsResult);
        }

        [HttpPost("product", Name = "GetProduct")]
        public IActionResult CreateProduct(int categoryId, [FromBody] ProductCreateModel product)
        {
            if(product == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var finalProduct = Mapper.Map<Entities.Product>(product);
            _productRepository.AddProduct(categoryId, finalProduct);
            if (!_productRepository.Save())
                return StatusCode(500, "A problem happened while handling your request");
            var createdProduct = Mapper.Map<Models.ProductModel>(finalProduct);
            return CreatedAtRoute("GetProduct", new {id = createdProduct.Id}, createdProduct);
        }

        [HttpPatch("{categoryId}/products/{id}")]
        public IActionResult UpdateProduct( int id,
            [FromBody] JsonPatchDocument<ProductCreateModel> patchDoc)
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
            var productToPatch = Mapper.Map<ProductCreateModel>(productEntity);
            patchDoc.ApplyTo(productToPatch, ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            TryValidateModel(productToPatch);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Mapper.Map(productToPatch, productEntity);
            if(!_productRepository.Save())
                return StatusCode(500, "A problem happened while handling your request");
            return NoContent();
        }

        [HttpDelete("/products/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            if(!_productRepository.ProductExists(id))
                return NotFound();
            var productEntity = _productRepository.GetProduct(id);
            if(productEntity == null)
                return NotFound();
            _productRepository.DeleteProduct(productEntity);
            if(!_productRepository.Save())
                return StatusCode(500, "A problem happened while handling your request");
            return NoContent();
        }
    }
}

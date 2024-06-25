using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService) 
        { 
            _productService = productService;
        }

        [HttpPost("[action]")]
        [Authorize("Admin&Seller")]
        public ActionResult<Product> CreateProduct([FromBody] ProductRequest productRequest) 
        {
            var sellerId = int.Parse(User.Claims.First(c => c.Type == "id").Value); // Obtener el ID del vendedor del token JWT
            var product = _productService.CreateProduct(productRequest , sellerId);
            return Ok(product);
        }

        [HttpPut("[action]/{id}")]
        [Authorize("Admin&Seller")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] ProductRequest productRequest)
        {
            try
            {
                var sellerId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
                await _productService.UpdateProduct(id, productRequest, sellerId);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("[action]/{id}")]
        [Authorize("Admin&Seller")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                var sellerId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
                await _productService.DeleteProduct(id, sellerId);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("[action]")]

        public ActionResult<IEnumerable<ProductDto>> GetAllProducts() 
        { 
            var products = _productService.GetAllProducts();
            return Ok(products);
        }
        [HttpGet("[action] / {id}")]
        public ActionResult<IEnumerable<ProductDto>> GetProductById(int id)
        {
            var products = _productService.GetProductById(id);
            return Ok(products);
        }
        //falta el buscador por nombre...
        
    }
}

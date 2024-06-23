using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepositoryBase<Product> _productRepository;

        public ProductService(IRepositoryBase<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            var products = await _productRepository.ListAsync();

            if (products == null || !products.Any())
            {
                return new List<ProductDto>();
            }

            var productDtos = ProductDto.CreateList(products);
            return productDtos;
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            var productDto = ProductDto.Create(product);
            return productDto;
        }

        public async Task<Product> CreateProduct(ProductRequest productRequest)
        {
            var product = new Product
            {
                Name = productRequest.Name,
                Price = productRequest.Price,
                Description = productRequest.Description,
                UserId = productRequest.UserId
            };

            return await _productRepository.AddAsync(product);
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            await _productRepository.DeleteAsync(product);
        }

        public async Task UpdateProduct(int id, ProductRequest productRequest)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            product.Name = productRequest.Name;
            product.Price = productRequest.Price;
            product.Description = productRequest.Description;
            product.UserId = productRequest.UserId;

            await _productRepository.UpdateAsync(product);
        }

    }
}

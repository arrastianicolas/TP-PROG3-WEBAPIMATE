using Application.Interfaces;
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
        private readonly IRepositoryBase<Product> _repository;
        private readonly IRepositoryBase<User> _userRepository;
        private readonly IProductRepository _productRepository;

        public ProductService(IRepositoryBase<Product> repositoryBase, IRepositoryBase<User> userRepository, IProductRepository productRepository)
        {
            _repository = repositoryBase;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task AddProductAsync(Product product, int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            if (user.UserType != "seller" && user.UserType != "admin")
            {
                throw new UnauthorizedAccessException("User is not authorized to add products");
            }

            product.UserId = userId; // Asignar el ID del usuario al producto
            await _repository.AddAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _repository.ListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public Product GetByNameAsync(string name)
        {
            return  _productRepository.GetByName(name);
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _repository.UpdateAsync(product);
        }
    }

}

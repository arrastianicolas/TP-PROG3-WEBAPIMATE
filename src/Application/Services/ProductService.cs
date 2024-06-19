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
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public ProductService(IProductRepository productRepository, IUserRepository userRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public void AddProduct(Product product, int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            if (user.UserType != "seller" && user.UserType != "admin")
            {
                throw new UnauthorizedAccessException("User is not authorized to add products");
            }

            product.UserId = userId; // Asignar el ID del usuario al producto
            _productRepository.AddProduct(product);
        }

        public void DeleteProduct(int id)
        {
            _productRepository.DeleteProduct(id);
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product GetById(int id)
        {
            return _productRepository.GetById(id);
        }

        public Product GetByName(string name)
        {
            return _productRepository.GetByName(name);
        }

        public void UpdateProduct(Product product)
        {
            _productRepository.UpdateProduct(product);
        }
    }

}

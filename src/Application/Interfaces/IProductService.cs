using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task AddProductAsync(Product product, int sellerId);
        Task DeleteProductAsync(int id);
        Task<List<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Product GetByNameAsync(string name);
        Task UpdateProductAsync(Product product);
    }
}

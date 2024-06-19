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
        void AddProduct(Product product, int sellerId);
        void DeleteProduct(int id);
        List<Product> GetAll();
        Product GetById(int id);
        Product GetByName(string name);
        void UpdateProduct(Product product);
    }
}

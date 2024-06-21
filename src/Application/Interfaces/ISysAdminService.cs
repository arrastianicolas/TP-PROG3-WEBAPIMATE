using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISysAdminService
    {
        // User Management
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<UserDto> GetUserById(int id);
        Task<User> CreateUser(User user);
        Task UpdateUser(int id);
        Task DeleteUser(int id);

        //// Product Management
        //IEnumerable<ProductDto> GetAllProducts();
        //ProductDto GetProductById(int id);
        //void CreateProduct(ProductDto product);
        //void UpdateProduct(ProductDto product);
        //void DeleteProduct(int id);
    }
}

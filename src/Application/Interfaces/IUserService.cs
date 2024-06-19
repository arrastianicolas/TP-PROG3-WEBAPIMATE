using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        void AddUser(User user);
        void DeleteUser(int id);
        List<User> GetAllUsers();
        User GetById(int id);
        void UpdateUser(User user);
    }
}

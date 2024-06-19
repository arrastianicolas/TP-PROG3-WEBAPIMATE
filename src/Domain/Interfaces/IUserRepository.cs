using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        User GetById(int id);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
        List<User> GetAllUsers();
    }
}

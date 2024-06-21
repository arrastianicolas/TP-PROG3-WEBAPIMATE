using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SysAdminService : ISysAdminService
    {
        private readonly IRepositoryBase<User> _userRepository;
        public SysAdminService(IRepositoryBase<User>  userRepository) 
        { 
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            // Obtener la lista de usuarios desde el repositorio
            var users = await _userRepository.ListAsync();

            // Verificar si la lista de usuarios es nula
            if (users == null)
            {
                return null; // O puedes lanzar una excepción si prefieres
            }

            // Convertir cada usuario en un UserDto
            var userDtos = users.Select(user => UserDto.Create(user)).ToList();

            return userDtos;
        }


        public async Task<UserDto> GetUserById(int id) 
        {
            // Obtener el usuario desde el repositorio
            var user = await _userRepository.GetByIdAsync(id);

            // Verificar si el usuario es nulo
            if (user == null)
            {
                return null; // O puedes lanzar una excepción si prefieres
            }

            // Convertir la entidad User a UserDto
            var userDto = UserDto.Create(user);

            return userDto;
        }

        public async Task<User> CreateUser(User user) 
        {
            return await _userRepository.AddAsync(user);
            
        }

        public async Task DeleteUser (int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            // Verificar si el usuario existe
            if (user == null)
            {
                throw new Exception("User not found."); // O manejarlo de la forma que prefieras
            }

            // Eliminar el usuario
            await _userRepository.DeleteAsync(user);
        }
       
        public async Task UpdateUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            // Verificar si el usuario existe
            if (user == null)
            {
                throw new Exception("User not found."); // O manejarlo de la forma que prefieras
            }

            // Editar el usuario
            await _userRepository.DeleteAsync(user);
        }
    }
}

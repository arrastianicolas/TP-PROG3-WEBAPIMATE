﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? UserType { get; set; } 
        public static UserDto Create(User user)
        {
            var dto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName, 
                Email = user.Email, 
                UserType = user.UserType
            };
            return dto;
       }
    }
}

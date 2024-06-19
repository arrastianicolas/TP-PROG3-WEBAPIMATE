using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Seller : User
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

    }
}
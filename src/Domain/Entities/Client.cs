using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.ConstrainedExecution;

namespace Domain.Entities
{
    public class Client : User
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
        public string Email { get; set; }

        public ICollection<Cart> Carts { get; set; } 
    }
}

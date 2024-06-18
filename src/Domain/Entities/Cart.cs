using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Domain.Entities
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public ICollection<Product> Products { get; set; }

        public int ClientId { get; set; } 
        public Client Client { get; set; } 
        public void AddProduct(Product product)
        {
            if (Products == null)
                Products = new List<Product>();

            Products.Add(product);
        }

        public decimal CalculateTotalPrice()
        {
            if (Products == null)
                return 0;

            return Products.Sum(p => p.Price);
        }
    }
}

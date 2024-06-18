using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> AddProductToCart(int clientId, int productId)
        {
            var client = await _context.Clients.Include(c => c.Carts)
                                               .ThenInclude(cart => cart.Products)
                                               .FirstOrDefaultAsync(c => c.Id == clientId);

            if (client == null) throw new Exception("Client not found.");

            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new Exception("Product not found.");

            var cart = client.Carts.FirstOrDefault() ?? new Cart { ClientId = clientId, Client = client };
            cart.AddProduct(product);

            if (!client.Carts.Contains(cart))
            {
                client.Carts.Add(cart);
            }

            _context.Update(client);
            await _context.SaveChangesAsync();

            return cart;
        }

        public async Task<Cart> GetCart(int clientId)
        {
            var client = await _context.Clients.Include(c => c.Carts)
                                               .ThenInclude(cart => cart.Products)
                                               .FirstOrDefaultAsync(c => c.Id == clientId);

            if (client == null) throw new Exception("Client not found.");

            var cart = client.Carts.FirstOrDefault();
            if (cart == null) throw new Exception("Cart not found.");

            return cart;
        }
    }
}

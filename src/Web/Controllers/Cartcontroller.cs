using Microsoft.AspNetCore.Mvc;
using Application.Services;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("{clientId}/add")]
        public async Task<IActionResult> AddToCart(int clientId, [FromBody] int productId)
        {
            try
            {
                var cart = await _cartService.AddProductToCart(clientId, productId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetCart(int clientId)
        {
            try
            {
                var cart = await _cartService.GetCart(clientId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

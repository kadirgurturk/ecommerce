using basic_ecommerce.Domain;
using basic_ecommerce.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace basic_ecommerce.Controller;

    [Route("api/[controller]")]
    [ApiController]
    public class EcommerceController : ControllerBase
    {
        private readonly EcommerceContext _context;

        public EcommerceController(EcommerceContext context)
        {
            _context = context;
        }

        // Yeni sipariş ekleme
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            if (order == null || order.OrderItems == null || !order.OrderItems.Any())
                return BadRequest("Sipariş verisi eksik veya boş.");
            
            var user = await _context.Users.FindAsync(order.UserId);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            decimal totalPrice = 0;
            
            foreach (var item in order.OrderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                {
                    return NotFound($"Ürün bulunamadı.");
                }

                if (product.StockQuantity < item.Quantity)
                {
                    return BadRequest($"Ürün için yeterli stok yok.");
                }

                totalPrice += product.Price * item.Quantity;
            }

            // Sipariş oluşturuluyor
            order.TotalAmount = totalPrice;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderDetails), new { id = order.Id }, order);
        }

        // Siparişleri listeleme
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Kullanıcı ID'si belirtilmeli.");

            // Kullanıcıya ait tüm siparişleri getirme
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();

            if (orders.Count == 0)
                return NotFound("Kullanıcıya ait sipariş bulunamadı.");

            return Ok(orders);
        }

        // Sipariş detayını getirme
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetails(string id)
        {
            // Sipariş ID'ye göre sipariş detayını getirme
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound("Sipariş bulunamadı.");

            return Ok(order);
        }

        // Sipariş silme
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return NotFound("Sipariş bulunamadı.");

            // Siparişi silme işlemi
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok("Sipariş başarıyla silindi.");
        }
    }

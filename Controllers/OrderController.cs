using CLDV_Ecommerce.Data;
using CLDV_Ecommerce.Models;
using CLDV_Ecommerce.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CLDV_Ecommerce.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEventPublisher _eventPublisher;



        public OrderController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IEventPublisher eventPublisher)
        {
            _context = context;
            _userManager = userManager;
            _eventPublisher = eventPublisher;
        }

        public async Task<IActionResult> Checkout()
        {
            var userId = _userManager.GetUserId(User);
            var cartItems = await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                TempData["Message"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = cartItems.Sum(ci => ci.Product.Price * ci.Quantity),
                Items = cartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Product.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            // Publish OrderPlaced event to Azure Queue
            await _eventPublisher.PublishAsync("OrderPlaced", new
            {
                order.Id,
                order.UserId,
                order.TotalAmount,
                order.OrderDate
            });

            return RedirectToAction("Confirmation", new { id = order.Id });
        }

        public async Task<IActionResult> Confirmation(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            return View(order);
        }


    }
}

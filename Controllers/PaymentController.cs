using CLDV_Ecommerce.Data;
using CLDV_Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CLDV_Ecommerce.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Confirm(int orderId)
        {
            var payment = new Payment
            {
                OrderId = orderId,
                StripePaymentIntentId = Guid.NewGuid().ToString(), // mock intent
                IsSuccessful = true,
                PaidAt = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Payment confirmed!";
            return RedirectToAction("Index", "Home");

        }
    }
}

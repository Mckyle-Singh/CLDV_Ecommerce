using Microsoft.AspNetCore.Identity;

namespace CLDV_Ecommerce.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public IdentityUser? User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}

using Microsoft.AspNetCore.Identity;

namespace CLDV_Ecommerce.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public IdentityUser? User { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }

    }
}

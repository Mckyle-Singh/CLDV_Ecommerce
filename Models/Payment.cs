namespace CLDV_Ecommerce.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public string StripePaymentIntentId { get; set; } = string.Empty;
        public bool IsSuccessful { get; set; }
        public DateTime PaidAt { get; set; }

    }
}

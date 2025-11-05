namespace CLDV_Ecommerce.Services
{
    public interface IEventPublisher
    {
        Task PublishAsync(string eventType, object payload);
    }
}

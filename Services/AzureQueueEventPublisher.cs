using Azure.Storage.Queues;
using System.Text.Json;

namespace CLDV_Ecommerce.Services
{
    public class AzureQueueEventPublisher :IEventPublisher
    {
        private readonly QueueClient _queueClient;

        public AzureQueueEventPublisher(string connectionString)
        {
            _queueClient = new QueueClient(connectionString, "order-events");
            _queueClient.CreateIfNotExists();
        }

        public async Task PublishAsync(string eventType, object payload)
        {
            var message = JsonSerializer.Serialize(new
            {
                EventType = eventType,
                Payload = payload,
                Timestamp = DateTime.UtcNow
            });

            await _queueClient.SendMessageAsync(message);
        }

    }
}

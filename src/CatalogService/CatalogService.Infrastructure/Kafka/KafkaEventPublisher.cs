using CatalogService.Domain;
using CatalogService.Domain.Aggregates.ProductEntity.Events;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CatalogService.Infrastructure.Kafka;

public class KafkaEventPublisher(IProducer<string, string> producer, ILogger<KafkaEventPublisher> logger)
{
    public async Task PublishProductUpdatedEvent(ProductUpdatedEvent message, CancellationToken cancellation = default)
    {
        try
        {
            using (logger.BeginScope("Kafka App Produce Sample Data"))
            {

                var json = JsonSerializer.Serialize(message, SystemJson.JsonSerializerOptions);
                var msg = new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = json
                };
                await producer.ProduceAsync("product", msg, cancellation);

            }
        }
        catch (Exception exception)
        {
            logger.LogError(exception, exception.Message);
        }
    }
}
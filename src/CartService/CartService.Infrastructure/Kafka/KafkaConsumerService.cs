using CartService.Domain.Handlers;
using CartService.Domain.Handlers.Events;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CartService.Infrastructure.Kafka;

public class KafkaConsumerService(IConsumer<string, string> consumer, ILogger<KafkaConsumerService> logger, IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {

            consumer.Subscribe("product");
            await Consume(stoppingToken).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    private async Task Consume(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = consumer.Consume(cancellationToken);

                if (consumeResult?.Message == null) continue;

                var message = JsonSerializer.Deserialize<ProductUpdatedEvent>(consumeResult.Message.Value);

                var scope = serviceProvider.CreateScope();
                var handler = scope.ServiceProvider.GetRequiredService<IProductUpdatedHandler>();
                await handler.Handle(message);
                consumer.StoreOffset(consumeResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}


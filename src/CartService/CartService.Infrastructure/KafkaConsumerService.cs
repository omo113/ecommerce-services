using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

namespace CartService.Infrastructure;

public class KafkaConsumerService(IConsumer<string, string> consumer) : BackgroundService
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

                consumer.StoreOffset(consumeResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
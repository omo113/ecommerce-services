using CatalogService.Domain.Aggregates.ProductEntity.Events;
using CatalogService.Domain.Models;
using CatalogService.Infrastructure.Kafka;
using CatalogService.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CatalogService.Infrastructure.BackgroundProcessors;

public class EventPublisher(KafkaEventPublisher kafkaEventPublisher, IServiceScopeFactory scopeFactory, ILogger<EventPublisher> logger) : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
        //var domainEventType = typeof(DomainEvent);
        //var types = Assembly.GetAssembly(typeof(Product))?
        //    .GetTypes()
        //    .Where(x => x.IsAssignableTo(domainEventType))
        //    .ToList();
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                var sp = scope.ServiceProvider;
                using var source = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);

                var dbContext = sp.GetRequiredService<CatalogDbContext>();

                var events = await dbContext.Set<OutboxMessage>().AsQueryable()
                    .Where(x => !x.IsPublished)
                    .OrderBy(x => x.Id)
                    .Take(10)
                    .ToListAsync(source.Token);
                foreach (var @event in events)
                {

                    switch (@event.MessageType)
                    {
                        case nameof(ProductUpdatedEvent):
                            @event.IsPublished = true;
                            var message = JsonSerializer.Deserialize<ProductUpdatedEvent>(@event.Message.ToString()!, new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            })!;
                            //await kafkaEventPublisher.PublishProductUpdatedEvent(message, source.Token);
                            break;
                        default:
                            // Continue for other event types
                            continue;
                    }

                    await dbContext.SaveChangesAsync(source.Token);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
            }

            await timer.WaitForNextTickAsync(stoppingToken);
        }
    }
}
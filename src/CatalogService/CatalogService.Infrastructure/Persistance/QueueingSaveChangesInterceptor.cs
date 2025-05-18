using CatalogService.Domain.Aggregates.ProductEntity;
using CatalogService.Domain.Aggregates.ProductEntity.Events;
using CatalogService.Infrastructure.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Infrastructure.Persistance;

public class QueueingSaveChangesInterceptor(IServiceProvider serviceProvider) : SaveChangesInterceptor
{
    private List<Product> _toEnqueue = new();


    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        //todo move this to outbox pattern
        var ctx = eventData.Context!;
        _toEnqueue = ctx.ChangeTracker
            .Entries<Product>()
            .Where(e =>
                e.State is EntityState.Modified)
            .Select(e => e.Entity)
            .ToList();

        return base.SavingChanges(eventData, result);
    }

    public override int SavedChanges(
        SaveChangesCompletedEventData eventData,
        int result)
    {
        //todo

        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        if (_toEnqueue.Any())
        {
            var kafkaProducer = serviceProvider.GetRequiredService<KafkaEventPublisher>();
            foreach (var entity in _toEnqueue)
            {
                await kafkaProducer.PublishProductUpdatedEvent(new ProductUpdatedEvent
                {
                    Price =
                    {
                        Amount =entity.Amount
                    }
                }, cancellationToken);
            }
        }

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}
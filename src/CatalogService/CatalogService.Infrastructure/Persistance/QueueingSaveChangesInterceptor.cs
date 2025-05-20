using CatalogService.Domain.Models;
using EcommerceServices.Shared;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CatalogService.Infrastructure.Persistance;

public class DomainEventInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        var ctx = eventData.Context!;
        var domainEntities = ctx.ChangeTracker
            .Entries()
            .Select(e => e.Entity)
            .OfType<IHasDomainEvent>();
        foreach (var entity in domainEntities)
        {
            ctx.Set<OutboxMessage>().AddRange(entity.PendingDomainEvents().Select(x => new OutboxMessage
            (
                Guid.NewGuid(),
                x.UId,
                x.GetType().ToString(),
                x
            )));
        }
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        var ctx = eventData.Context!;
        var domainEntities = ctx.ChangeTracker
            .Entries()
            .Select(e => e.Entity)
            .OfType<IHasDomainEvent>();
        foreach (var entity in domainEntities)
        {
            ctx.Set<OutboxMessage>().AddRange(entity.PendingDomainEvents().Select(x => new OutboxMessage
            (
                Guid.NewGuid(),
                x.UId,
                x.GetType().ToString(),
                x
            )));
        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
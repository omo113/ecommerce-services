using CatalogService.Domain.Models;
using EcommerceServices.Shared;
using Microsoft.EntityFrameworkCore.Diagnostics;

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
            {
                Message = x,
                Id = x.UId,
                MessageType = x.GetType().ToString(),
                IsPublished = false,
            }));
        }
        return base.SavingChanges(eventData, result);
    }


}
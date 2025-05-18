namespace EcommerceServices.Shared;

public class AggregateRoot : Entity, IHasDomainEvent
{
    public Guid UId { get; protected init; }
    protected List<DomainEvent> DomainEvents { get; } = new();
    public IReadOnlyList<DomainEvent> PendingDomainEvents()
    {
        return DomainEvents.Where(x => !x.IsPublished).ToList();
    }
    protected virtual void Raise(DomainEvent @event)
    {
        @event.UId = UId;
        DomainEvents.Add(@event);
    }
}
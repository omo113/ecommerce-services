using System.Text.Json.Serialization;

namespace EcommerceServices.Shared;

public abstract class DomainEvent : IDomainEvent
{
    public Guid UId { get; set; }

    public DateTimeOffset DateOccurred { get; } = TimeProvider.System.GetUtcNow();
    [JsonIgnore]
    public bool IsPublished { get; private set; }


}
public interface IHasDomainEvent
{
    IReadOnlyList<DomainEvent> PendingDomainEvents();
}

public interface IDomainEvent
{
    public Guid UId { get; }
    public DateTimeOffset DateOccurred { get; }
    public bool IsPublished { get; }
}
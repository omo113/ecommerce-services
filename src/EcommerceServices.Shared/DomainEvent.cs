namespace EcommerceServices.Shared;

public abstract class DomainEvent
{
    public Guid UId { get; set; }

    public DateTimeOffset DateOccurred { get; } = TimeProvider.System.GetUtcNow();
}
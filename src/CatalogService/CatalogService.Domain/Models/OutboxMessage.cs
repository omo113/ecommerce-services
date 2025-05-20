namespace CatalogService.Domain.Models;

public class OutboxMessage
{
    public required Guid Id { get; init; }
    public required Guid EventId { get; init; }
    public required string MessageType { get; init; }
    public required object Message { get; init; }
    public DateTimeOffset ProcessedOnUtc { get; init; }
    public required bool IsPublished { get; set; }
}
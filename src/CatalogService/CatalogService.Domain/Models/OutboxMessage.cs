using System.Text.Json;

namespace CatalogService.Domain.Models;

public class OutboxMessage
{
    private OutboxMessage()
    {

    }

    public OutboxMessage(
        Guid id,
        Guid eventId,
        string messageType,
        object message)
    {
        Id = id;
        EventId = eventId;
        MessageType = messageType;
        Message = JsonSerializer.SerializeToElement(message, SystemJson.JsonSerializerOptions);
    }

    public Guid Id { get; private set; }
    public Guid EventId { get; private set; }
    public string MessageType { get; private set; }
    public JsonElement Message { get; private set; }
    public DateTimeOffset ProcessedOnUtc { get; set; }
    public bool IsPublished { get; set; }
}
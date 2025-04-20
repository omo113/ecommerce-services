namespace EcommerceServices.Shared;

public abstract class Entity
{
    public int Id { get; protected set; }
    public DateTimeOffset CreateDate { get; init; }
    public DateTimeOffset? LastChangeDate { get; protected set; }
}
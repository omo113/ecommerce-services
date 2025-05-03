namespace EcommerceServices.Shared;



public record Query
{
    public required int PageIndex { get; init; }
    public required int PageSize { get; init; }
}
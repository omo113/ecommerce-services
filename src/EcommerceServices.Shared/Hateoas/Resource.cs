namespace EcommerceServices.Shared.Hateoas;

public record Resource<T>(T Data, IEnumerable<Link> Links);
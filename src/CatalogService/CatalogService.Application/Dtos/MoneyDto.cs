using EcommerceServices.Shared;

namespace CatalogService.Application.Dtos;

public record MoneyDto(decimal Amount, Currency Currency);
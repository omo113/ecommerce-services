using CatalogService.Domain.ValueObjects;

namespace CartService.Application.Dtos;

public record MoneyDto(decimal Amount, Currency Currency);
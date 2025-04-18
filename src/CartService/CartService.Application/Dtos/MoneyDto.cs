using CartService.Domain.Aggregates;

namespace CartService.Application.Dtos;

public record MoneyDto(decimal Amount, Currency Currency);
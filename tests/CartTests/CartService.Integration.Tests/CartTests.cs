using CartService.Application.Commands.CartCommands;
using CartService.Application.Dtos;
using EcommerceServices.Shared;
using CartService.Integration.Tests.Abstractions;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Collections.Generic;
using Xunit;

namespace CartService.Integration.Tests;

public class CartTests(CartWebAppFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task GetCarts_InitiallyEmpty_ReturnsOkAndEmptyList()
    {
        // Act
        var response = await Client.GetAsync("/v1/cart");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var carts = await response.Content.ReadFromJsonAsync<List<CartDto>>()!;
        carts.ShouldBeEmpty();
    }

    [Fact]
    public async Task AddItem_ReturnsOkTrue()
    {
        // Arrange
        var command = new CreateCartCommand(
            "cart1", "Item1", new ImageDto("http://img", "alt"),
            new MoneyDto(10m, Currency.USD), 1);

        // Act
        var response = await Client.PostAsJsonAsync("/v1/cart", command);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<bool>()!;
        result.ShouldBeTrue();
    }

    [Fact]
    public async Task GetCarts_AfterAdd_ReturnsListWithItem()
    {
        // Arrange
        var command = new CreateCartCommand(
            "cart2", "Item2", new ImageDto("http://img2", "alt2"),
            new MoneyDto(20m, Currency.EUR), 2);
        var post = await Client.PostAsJsonAsync("/v1/cart", command);
        post.EnsureSuccessStatusCode();

        // Act
        var response = await Client.GetAsync("/v1/cart");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var carts = await response.Content.ReadFromJsonAsync<List<CartDto>>()!;
        carts.Count.ShouldBe(1);
        var cart = carts[0];
        cart.Id.ShouldBe("cart2");
        cart.Name.ShouldBe("Item2");
        cart.Price.Amount.ShouldBe(20m);
        cart.Price.Currency.ShouldBe(Currency.EUR);
        cart.Image!.Url.ShouldBe("http://img2");
        cart.Image.Alt.ShouldBe("alt2");
        cart.Quantity.ShouldBe(2);
    }

    [Fact]
    public async Task RemoveItem_ReturnsOkTrueThenListEmpty()
    {
        // Arrange
        var command = new CreateCartCommand(
            "cart3", "Item3", new ImageDto("http://img3", "alt3"),
            new MoneyDto(30m, Currency.GBP), 3);
        var post = await Client.PostAsJsonAsync("/v1/cart", command);
        post.EnsureSuccessStatusCode();

        // Act: delete
        var respDelete = await Client.DeleteAsync("/v1/cart?id=cart3");

        // Assert
        respDelete.StatusCode.ShouldBe(HttpStatusCode.OK);
        var delResult = await respDelete.Content.ReadFromJsonAsync<bool>()!;
        delResult.ShouldBeTrue();
        var response = await Client.GetAsync("/v1/cart");
        var carts = await response.Content.ReadFromJsonAsync<List<CartDto>>()!;
        carts.ShouldBeEmpty();
    }
}
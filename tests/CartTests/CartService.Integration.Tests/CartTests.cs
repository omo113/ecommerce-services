using CartService.Application.Commands.CartCommands;
using CartService.Application.Dtos;
using CartService.Integration.Tests.Abstractions;
using EcommerceServices.Shared;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Linq;

namespace CartService.Integration.Tests;

public class CartTests(CartWebAppFactory factory) : IntegrationTestBase(factory)
{
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
        cart.Items.Count.ShouldBe(1);
        var item = cart.Items[0];
        item.Name.ShouldBe("Item2");
        item.Price.Amount.ShouldBe(20m);
        item.Price.Currency.ShouldBe(Currency.EUR);
        item.Image!.Url.ShouldBe("http://img2");
        item.Image.Alt.ShouldBe("alt2");
        item.Quantity.ShouldBe(2);
    }

    [Fact]
    public async Task RemoveItem_ReturnsOkTrueThenListEmpty()
    {
        // Arrange
        var command = new CreateCartCommand(
            "cart3", "Item3", new ImageDto("http://img3", "alt3"),
            new MoneyDto(30m, Currency.GBP), 3);
        await Client.PostAsJsonAsync("/v1/cart", command);

        // get itemId
        var getCart = await Client.GetAsync("/v1/cart/cart3");
        getCart.EnsureSuccessStatusCode();
        var cartDto = await getCart.Content.ReadFromJsonAsync<CartDto>()!;
        var itemId = cartDto.Items.First().ItemId;

        // Act: delete
        var respDelete = await Client.DeleteAsync($"/v1/cart/cart3/items/{itemId}");

        // Assert
        respDelete.StatusCode.ShouldBe(HttpStatusCode.OK);
        var delResult = await respDelete.Content.ReadFromJsonAsync<bool>()!;
        delResult.ShouldBeTrue();

        var after = await Client.GetAsync("/v1/cart/cart3");
        after.EnsureSuccessStatusCode();
        var updated = await after.Content.ReadFromJsonAsync<CartDto>()!;
        updated.Items.ShouldBeEmpty();
    }
}
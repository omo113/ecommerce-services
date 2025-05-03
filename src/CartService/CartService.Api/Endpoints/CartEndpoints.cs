using CartService.Api.Extensions;
using CartService.Application.Commands.CartCommands;
using CartService.Application.Queries.CartQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CartService.Api.Endpoints;

public static class CartEndpoints
{
    private const string BasePath = "/v1/cart/";
    private const string Tags = "Cart";
    private const string BasePathV2 = "/v2/cart/";

    public static IEndpointRouteBuilder MapCartServiceEndpoints(this IEndpointRouteBuilder app)
    {
        var cart = app.MapGroup(BasePath)
            .WithOpenApi(o =>
            {
                o.Summary = "Cart API v1";
                return o;
            });
        cart.MapGet("", GetCarts)
            .WithName("Getcartinfo")
            .WithTags(Tags)
            .WithOpenApi(o =>
            {
                o.Summary = "Version 1: Get all carts";
                o.Description = "Retrieves all carts, each with its items list.";
                return o;
            });

        cart.MapGet("{id}", GetCartById)
            .WithName("GetCartById")
            .WithTags(Tags)
            .WithOpenApi(o =>
            {
                o.Summary = "Version 1: Get cart by ID";
                o.Description = "Retrieves the cart model (ID + items) for the specified cart ID.";
                return o;

            });

        cart.MapPost("", AddItem)
            .WithName("AddItemToCart")
            .WithTags(Tags)
            .WithOpenApi(o =>
            {
                o.Summary = "Version 1: Add item to cart";
                o.Description = "Adds an item to the specified cart (creates cart if not exists).";
                return o;
            });

        cart.MapDelete("{id}/items/{itemId}", RemoveCartItem)
            .WithName("DeleteCartItem")
            .WithTags(Tags)
            .WithOpenApi(o =>
            {
                o.Summary = "Version 1: Delete item from cart";
                o.Description = "Removes a single item from the specified cart.";
                return o;
            });

        return app;
    }

    public static IEndpointRouteBuilder MapCartServiceV2Endpoints(this IEndpointRouteBuilder app)
    {
        var cart2 = app.MapGroup(BasePathV2)
            .WithOpenApi(o =>
            {
                o.Summary = "Cart API v2";
                return o;
            });
        cart2.MapGet("{id}", GetCartItemsV2)
            .WithName("GetCartItemsV2")
            .WithTags(Tags)
            .WithOpenApi(o =>
            {
                o.Summary = "Version 2: Get cart items";
                o.Description = "Retrieves only the list of cart items for the specified cart ID.";
                return o;
            });

        cart2.MapPost("", AddItem)
            .WithName("AddItemToCartV2")
            .WithTags(Tags)
            .WithOpenApi(o =>
            {
                o.Summary = "Version 2: Add item to cart";
                o.Description = "Adds an item to the specified cart (cart created if not exists).";
                return o;
            });

        cart2.MapDelete("{id}/items/{itemId}", RemoveCartItem)
            .WithName("DeleteCartItemV2")
            .WithTags(Tags)
            .WithOpenApi(o =>
            {
                o.Summary = "Version 2: Delete item from cart";
                o.Description = "Removes a single item from the specified cart.";
                return o;
            });

        return app;
    }

    public static async Task<IResult> GetCarts(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken) =>
        (await mediator.Send(new CartsQuery(), cancellationToken)).ToApiResult();

    public static async Task<IResult> GetCartById(
        [FromRoute(Name = "id")] string id,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken) =>
        (await mediator.Send(new GetCartQuery(id), cancellationToken)).ToApiResult();

    public static async Task<IResult> AddItem(
        [FromBody] CreateCartCommand command,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken) =>
       (await mediator.Send(command, cancellationToken)).ToApiResult();

    public static async Task<IResult> RemoveCartItem(
        [FromRoute(Name = "id")] string id,
        [FromRoute(Name = "itemId")] string itemId,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken) =>
        (await mediator.Send(new DeleteCartCommand(id, itemId), cancellationToken)).ToApiResult();

    public static async Task<IResult> GetCartItemsV2(
        [FromRoute(Name = "id")] string id,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken) =>
        (await mediator.Send(new GetCartItemsQuery(id), cancellationToken)).ToApiResult();
}



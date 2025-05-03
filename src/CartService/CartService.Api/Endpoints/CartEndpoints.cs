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
        var cart = app.MapGroup(BasePath);
        cart.MapGet("", GetCarts)
            .WithName("Getcartinfo")
            .WithTags(Tags)
            .WithOpenApi();

        cart.MapGet("{id}", GetCartById)
            .WithName("GetCartById")
            .WithTags(Tags)
            .WithOpenApi();

        cart.MapPost("", AddItem)
            .WithName("AddItemToCart")
            .WithTags(Tags)
            .WithOpenApi();

        cart.MapDelete("{id}/items/{itemId}", RemoveCartItem)
            .WithName("DeleteCartItem")
            .WithTags(Tags)
            .WithOpenApi();

        return app;
    }

    // Version 2: return list of cart items only
    public static IEndpointRouteBuilder MapCartServiceV2Endpoints(this IEndpointRouteBuilder app)
    {
        var cart2 = app.MapGroup("/v2/cart/");
        cart2.MapGet("{id}", GetCartItemsV2)
            .WithName("GetCartItemsV2")
            .WithTags(Tags)
            .WithOpenApi();

        cart2.MapPost("", AddItem)
            .WithName("AddItemToCartV2")
            .WithTags(Tags)
            .WithOpenApi();

        cart2.MapDelete("{id}/items/{itemId}", RemoveCartItem)
            .WithName("DeleteCartItemV2")
            .WithTags(Tags)
            .WithOpenApi();

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



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

    public static IEndpointRouteBuilder MapCartServiceEndpoints(this IEndpointRouteBuilder app)
    {
        var cart = app.MapGroup(BasePath);
        cart.MapGet("", GetCarts)
            .WithName("Getcartinfo")
            .WithTags(Tags)
            .WithOpenApi();

        cart.MapPost("", AddItem)
            .WithName("AddItemToCart")
            .WithTags(Tags)
            .WithOpenApi();

        cart.MapDelete("", RemoveItem)
            .WithName("DeleteItemFromCart")
            .WithTags(Tags)
            .WithOpenApi();

        return app;
    }

    public static async Task<IResult> GetCarts(
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken) =>
        (await mediator.Send(new CartsQuery(), cancellationToken)).ToApiResult();

    public static async Task<IResult> AddItem(
        [FromBody] CreateCartCommand command,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken) =>
       (await mediator.Send(command, cancellationToken)).ToApiResult();

    public static async Task<IResult> RemoveItem(
        [FromQuery(Name = "id")] string id,
        [FromServices] IMediator mediator,
        CancellationToken cancellationToken) =>
        (await mediator.Send(new DeleteCartCommand(id), cancellationToken)).ToApiResult();
}



using CatalogService.Application.Commands.ProductCommands;
using CatalogService.Application.Dtos;
using CatalogService.Application.Queries.ProductQueries;
using EcommerceServices.Shared.Hateoas;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProductsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IEnumerable<ProductDto>> GetAll([FromQuery] GetAllProductsQueryModel query) =>
        await _mediator.Send(new GetAllProductsQuery(query));

    [HttpGet("{id}")]
    public async Task<ActionResult<Resource<ProductDto>>> GetById(int id)
    {
        var dto = await _mediator.Send(new GetProductByIdQuery(id));
        if (dto is null) return NotFound();
        var resource = new Resource<ProductDto>(
            dto,
            [
                new Link("self", Url.Action(nameof(GetById), new { id })!, "GET"),
                new Link("update", Url.Action(nameof(Update), new { id })!, "PUT"),
                new Link("delete", Url.Action(nameof(Delete), new { id })!, "DELETE"),
                new Link("all", Url.Action(nameof(GetAll))!, "GET")
            ]
        );
        return Ok(resource);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create([FromBody] AddProductCommand command)
    {
        var dto = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductCommand command)
    {
        if (id != command.Id) return BadRequest();
        var success = await _mediator.Send(command);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _mediator.Send(new DeleteProductCommand(id));
        return success ? NoContent() : NotFound();
    }
}
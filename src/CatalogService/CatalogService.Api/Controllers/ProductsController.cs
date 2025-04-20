using MediatR;
using Microsoft.AspNetCore.Mvc;
using CatalogService.Application.Commands.ProductCommands;
using CatalogService.Application.Queries.ProductQueries;
using CatalogService.Application.Dtos;

namespace CatalogService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProductsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IEnumerable<ProductDto>> GetAll() =>
        await _mediator.Send(new GetAllProductsQuery());

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var dto = await _mediator.Send(new GetProductByIdQuery(id));
        return dto is not null ? Ok(dto) : NotFound();
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
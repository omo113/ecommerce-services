using CatalogService.Application.Commands.CategoryCommands;
using CatalogService.Application.Dtos;
using CatalogService.Application.Queries.CategoryQueries;
using EcommerceServices.Shared.Hateoas;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    public CategoriesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IEnumerable<CategoryDto>> GetAll() =>
        await _mediator.Send(new GetAllCategoriesQuery());

    [HttpGet("{id}")]
    public async Task<ActionResult<Resource<CategoryDto>>> GetById(int id)
    {
        var dto = await _mediator.Send(new GetCategoryByIdQuery(id));
        if (dto is null) return NotFound();
        var resource = new Resource<CategoryDto>(
            dto,
            new[]
            {
                new Link("self", Url.Action(nameof(GetById), new { id })!, "GET"),
                new Link("update", Url.Action(nameof(Update), new { id })!, "PUT"),
                new Link("delete", Url.Action(nameof(Delete), new { id })!, "DELETE"),
                new Link("all", Url.Action(nameof(GetAll))!, "GET")
            }
        );
        return Ok(resource);
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] AddCategoryCommand command)
    {
        var dto = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryCommand command)
    {
        if (id != command.Id) return BadRequest();
        var success = await _mediator.Send(command);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _mediator.Send(new DeleteCategoryCommand(id));
        return success ? NoContent() : NotFound();
    }
}
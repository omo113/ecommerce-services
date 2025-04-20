using MediatR;
using Microsoft.AspNetCore.Mvc;
using CatalogService.Application.Commands.CategoryCommands;
using CatalogService.Application.Queries.CategoryQueries;
using CatalogService.Application.Dtos;

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
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        var dto = await _mediator.Send(new GetCategoryByIdQuery(id));
        return dto is not null ? Ok(dto) : NotFound();
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
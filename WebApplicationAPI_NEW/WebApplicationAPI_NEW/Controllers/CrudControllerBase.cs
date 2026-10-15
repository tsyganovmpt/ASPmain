using ApiAsp0.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiAsp0.Controllers;

[ApiController]
public abstract class CrudControllerBase<TEntity, TReadDto, TCreateDto, TUpdateDto, TPatchDto> : ControllerBase
    where TEntity : class
{
    private readonly ICrudService<TEntity, TReadDto, TCreateDto, TUpdateDto, TPatchDto> _service;

    protected CrudControllerBase(ICrudService<TEntity, TReadDto, TCreateDto, TUpdateDto, TPatchDto> service)
        => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
    {
        if (page < 1 || pageSize < 1) return BadRequest(new { message = "page и pageSize должны быть положительными числами." });
        var result = await _service.GetPageAsync(page, pageSize, ct);
        Response.Headers["X-Total-Count"] = result.TotalCount.ToString();
        return Ok(result.Items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct = default)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TCreateDto dto, CancellationToken ct = default)
    {
        var result = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result.Item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TUpdateDto dto, CancellationToken ct = default)
        => await _service.UpdateAsync(id, dto, ct) ? NoContent() : NotFound();

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> Patch(int id, [FromBody] TPatchDto dto, CancellationToken ct = default)
        => await _service.PatchAsync(id, dto, ct) ? NoContent() : NotFound();

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct = default)
        => await _service.DeleteAsync(id, ct) ? NoContent() : NotFound();
}

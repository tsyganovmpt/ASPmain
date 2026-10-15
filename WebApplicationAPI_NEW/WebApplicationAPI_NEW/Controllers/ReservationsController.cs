using ApiAsp0.DTO.DTOreservation;
using ApiAsp0.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiAsp0.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ReservationsController : ControllerBase
{
    private readonly IReservationService _service;
    public ReservationsController(IReservationService service) => _service = service;

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var reservation = await _service.GetByIdAsync(id, ct);
        return reservation is null ? NotFound() : Ok(reservation);
    }

    [HttpPost]
    public async Task<IActionResult> Reserve([FromBody] CreateReservationDto dto, CancellationToken ct)
    {
        var result = await _service.ReserveAsync(dto, ct);
        if (result.Success)
            return CreatedAtAction(nameof(GetById), new { id = result.Reservation!.IdReservation }, result.Reservation);
        if (result.NotFound)
            return NotFound(new { message = result.Error });
        if (result.Error?.StartsWith("Недостаточно", StringComparison.OrdinalIgnoreCase) == true)
            return Conflict(new { message = result.Error });
        return BadRequest(new { message = result.Error });
    }

    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id, CancellationToken ct)
    {
        var result = await _service.CancelAsync(id, ct);
        if (result.Success) return Ok(result.Reservation);
        return NotFound(new { message = result.Error });
    }
}

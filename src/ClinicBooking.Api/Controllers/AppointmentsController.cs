using Microsoft.AspNetCore.Mvc;
using ClinicBooking.Application.Interfaces;
using ClinicBooking.Application.DTOs;

namespace ClinicBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController(IAppointmentService service) : ControllerBase
{
    [HttpGet("by-clinic/{clinicId:long}/date/{date}")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByClinicAndDate(long clinicId, DateOnly date, CancellationToken ct)
        => Ok(await service.GetByClinicAndDateAsync(clinicId, date, ct));

    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> Book([FromBody] CreateAppointmentRequest req, CancellationToken ct)
        => Ok(await service.BookAsync(req, ct));
}

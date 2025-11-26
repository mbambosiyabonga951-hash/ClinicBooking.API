using Microsoft.AspNetCore.Mvc;
using ClinicBooking.Application.Interfaces;
using ClinicBooking.Application.DTOs;

namespace ClinicBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
/// <summary>
/// Controller for managing clinic appointments.
/// </summary>
public class AppointmentsController : ControllerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppointmentsController"/> class.
    /// </summary>
    /// <param name="service">The appointment service.</param>
    public AppointmentsController(IAppointmentService service) : base()
    {
        this.service = service;
    }

    private readonly IAppointmentService service;

    [HttpGet("by-clinic/{clinicId:long}/date/{date}")]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByClinicAndDate(long clinicId, DateOnly date, CancellationToken ct)
    {
        try
        {
            var appointments = await service.GetByClinicAndDateAsync(clinicId, date, ct);

            if (appointments == null || !appointments.Any())
                return NotFound("No appointments found.");

            return Ok(appointments);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An error occurred while retrieving appointments.", details = ex.Message });
        }
    }

    [HttpPost]

    public async Task<ActionResult<AppointmentDto>> Book([FromBody] CreateAppointmentRequest req, CancellationToken ct)
    {
        if (req == null)
            return BadRequest("Request body cannot be null.");

        try
        {
            var appointment = await service.BookAsync(req, ct);

            if (appointment == null)
                return BadRequest("Failed to book appointment.");

            return CreatedAtAction(nameof(GetByClinicAndDate), new { clinicId = req.ClinicId, date = req.Date }, appointment);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An unexpected error occurred while booking the appointment.", details = ex.Message });
        }
    }
}

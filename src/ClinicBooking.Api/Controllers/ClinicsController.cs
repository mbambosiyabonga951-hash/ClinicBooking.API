using Microsoft.AspNetCore.Mvc;
using ClinicBooking.Application.Interfaces;
using ClinicBooking.Application.DTOs;

namespace ClinicBooking.Api.Controllers;

/// <summary>
/// API controller for managing clinics.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClinicsController : ControllerBase
{
    private readonly IClinicService service;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClinicsController"/> class.
    /// </summary>
    /// <param name="service">The clinic service.</param>
    public ClinicsController(IClinicService service)
    {
        this.service = service;
    }

    /// <summary>
    /// Gets all clinics.
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>A list of clinics.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClinicDto>>> GetAll(CancellationToken ct)
    {
        try
        {
            var clinics = await service.GetAllAsync(ct);

            if (clinics == null || !clinics.Any())
                return NotFound("No clinics found.");

            return Ok(clinics);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An error occurred while retrieving clinics.", details = ex.Message });
        }
    }
}

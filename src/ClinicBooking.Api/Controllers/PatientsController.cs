using Microsoft.AspNetCore.Mvc;
using ClinicBooking.Application.Interfaces;
using ClinicBooking.Application.DTOs;

namespace ClinicBooking.Api.Controllers;

/// <summary>
/// Controller for managing patient-related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService service;
    private object _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PatientsController"/> class.
    /// </summary>
    /// <param name="service">The patient service.</param>
    public PatientsController(IPatientService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientDto>>> GetAll(CancellationToken ct)
    {
        try
        {
            var patients = await service.GetAllAsync(ct);

            if (patients == null || !patients.Any())
                return NotFound("No patients found.");

            return Ok(patients);
        }
        catch (Exception ex)
        {

            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An error occurred while retrieving patients.", details = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<PatientDto>> Create([FromBody] CreatePatientRequest request, CancellationToken ct)
    {
        if (request == null)
            return BadRequest("Request body cannot be null.");

        try
        {
            var patient = await service.CreateAsync(request, ct);

            if (patient == null)
                return BadRequest("Failed to create patient.");

            return CreatedAtAction(nameof(GetAll), new { id = patient.Id }, patient);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An unexpected error occurred while creating the patient.", details = ex.Message });
        }
    }

}

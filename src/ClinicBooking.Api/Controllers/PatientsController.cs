using Microsoft.AspNetCore.Mvc;
using ClinicBooking.Application.Interfaces;
using ClinicBooking.Application.DTOs;

namespace ClinicBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController(IPatientService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientDto>>> GetAll(CancellationToken ct)
        => Ok(await service.GetAllAsync(ct));

    [HttpPost]
    public async Task<ActionResult<PatientDto>> Create([FromBody] CreatePatientRequest request, CancellationToken ct)
        => Ok(await service.CreateAsync(request, ct));
}

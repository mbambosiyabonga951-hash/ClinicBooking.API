using Microsoft.AspNetCore.Mvc;
using ClinicBooking.Application.Interfaces;
using ClinicBooking.Application.DTOs;

namespace ClinicBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClinicsController(IClinicService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClinicDto>>> GetAll(CancellationToken ct)
        => Ok(await service.GetAllAsync(ct));
}

using Microsoft.AspNetCore.Mvc;
using ClinicBooking.Application.Interfaces;
using ClinicBooking.Application.Requests;
using ClinicBooking.Application.DTOs;


namespace ClinicBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProvidersController : Controller
    {
        private readonly IProviderService _service;
        private readonly ILogger<ProvidersController> _logger;

        public ProvidersController(IProviderService service, ILogger<ProvidersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProviderDto>>> GetAll(CancellationToken ct)
        {
            try
            {
                var providers = await _service.GetAllAsync(ct);
                if (providers == null || !providers.Any())
                    return NotFound("No providers found.");
                return Ok(providers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving providers.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while retrieving providers.", details = ex.Message });
            }

        }


        [HttpPost]
        public async Task<ActionResult<ProviderDto>> Create([FromBody] UpsertProviderRequest request, CancellationToken ct)
        {
            if (request == null)
                return BadRequest("Request body cannot be null.");
            try
            {
                var provider = await _service.CreateAsync(request, ct);
                if (provider == null)
                    return BadRequest("Failed to create provider.");
                return CreatedAtAction(nameof(GetAll), new { id = provider.Id }, provider);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument while creating provider.");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating the provider.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred while creating the provider.", details = ex.Message });
            }
        }
    }
}

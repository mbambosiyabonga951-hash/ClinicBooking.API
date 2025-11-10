using ClinicBooking.Application.DTOs;
using ClinicBooking.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimeslotsController : Controller
    {
        private readonly ITimeslotService service;
        //private readonly ILogger<TimeslotsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeslotsController"/> class.
        /// </summary>
        /// <param name="svc">The timeslot service.</param>
        //public TimeslotsController(ITimeslotService svc, ILogger<TimeslotsController> logger)
        public TimeslotsController(ITimeslotService svc)
        {
            service = svc;
            //_logger = logger;
        }

        [HttpGet("by-provider/{providerId:long}/date/{date}")]
        public async Task<ActionResult<IEnumerable<TimeslotDto>>> GetByProviderAndDate(int providerId, DateOnly date, CancellationToken ct)
        {
            return Ok(await service.GetByProviderAndDateAsync(providerId, date, ct));
        }
    }
}

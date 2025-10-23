using ClinicBooking.Application.DTOs;
using ClinicBooking.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace ClinicBooking.Application.Services
{
    public class ClinicService : IClinicService
    {
        private readonly IClinicRepository _clinicRepository;
        private readonly ILogger<ClinicService> _logger;

        public ClinicService(IClinicRepository clinicRepository, ILogger<ClinicService> logger)
        {
            _clinicRepository = clinicRepository;
            _logger = logger;
        }
        public async Task<IEnumerable<ClinicDto>> GetAllAsync(CancellationToken ct)
        {
            try
            {
                var items = await _clinicRepository.GetAllAsync(ct);

                return items.Select(c => new ClinicDto(c.Id, c.Name, c.Address));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return Enumerable.Empty<ClinicDto>();
            }
        }
    }
}

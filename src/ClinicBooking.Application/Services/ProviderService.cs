using ClinicBooking.Application.DTOs;
using ClinicBooking.Application.Interfaces;
using ClinicBooking.Application.Requests;
using Microsoft.Extensions.Logging;

namespace ClinicBooking.Application.Services
{


    public class ProviderService : IProviderService
    {
        private readonly ILogger<ProviderService> _logger;
        private readonly IProviderRepository _providerRepository;
        public ProviderService(ILogger<ProviderService> logger, IProviderRepository providerRepository)
        {
            _logger = logger;
            _providerRepository = providerRepository;
        }

        public async Task<IEnumerable<ProviderDto>> GetAllAsync(CancellationToken ct)
        {
            _logger.LogInformation("Fetching all providers.");
            var providers = await _providerRepository.GetAllAsync(ct);
            return providers.Select(p => new ProviderDto
            {
                Id = p.Id,
                Name = p.Name,
                Specialty = p.Specialty,
                ClinicId = p.ClinicId
            });


        }
        public async Task<ProviderDto> CreateAsync(UpsertProviderRequest request, CancellationToken ct)
        {
            _logger.LogInformation("Creating a new provider with name: {FullName}", request.FullName);
            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                _logger.LogWarning("Provider creation failed: FullName is required.");
                throw new ArgumentException("FullName is required.");
            }
            var provider = await _providerRepository.CreateAsync(request.FullName, request.Specialty, ct);
            _logger.LogInformation("Provider created successfully with ID: {ProviderId}", provider.Id);
            return new ProviderDto
            {
                Id = provider.Id,
                Name = provider.Name,
                Specialty = provider.Specialty
            };
        }


    }
}

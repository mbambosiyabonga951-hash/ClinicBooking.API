using ClinicBooking.Application.DTOs;
using ClinicBooking.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace ClinicBooking.Application.Services
{

    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly ILogger<PatientService> _logger;

        public PatientService(IPatientRepository patientRepository, ILogger<PatientService>  logger) 
        { 
        _patientRepository = patientRepository;
         _logger = logger;
        }
        public async Task<IEnumerable<PatientDto>> GetAllAsync(CancellationToken ct)
        {
            try
            {
                var items = await _patientRepository.GetAllAsync(ct);
                return items.Select(p => new PatientDto(p.Id, p.FirstName, p.LastName, p.Email));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving patients.");
                throw;
            }
        }
        public async Task<PatientDto> CreateAsync(CreatePatientRequest request, CancellationToken ct)
        {
            try
            {
                var entity = await _patientRepository.CreateAsync(request.FirstName, request.LastName, request.Email, ct);
                return new PatientDto(entity.Id, entity.FirstName, entity.LastName, entity.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a patient.");
                throw;
            }
        }
    }
}
using ClinicBooking.Application.DTOs;
using ClinicBooking.Application.Interfaces;
using ClinicBooking.Application.Providers;
using Microsoft.Extensions.Logging;

namespace ClinicBooking.Application.Services
{

    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(IAppointmentRepository repo, ILogger<AppointmentService> log)
        {
            _repo = repo; _logger = log;
        }
        public async Task<IEnumerable<AppointmentDto>> GetByClinicAndDateAsync(long clinicId, DateOnly date, CancellationToken ct)
        {
            try
            {
                var items = await _repo.GetByClinicAndDateAsync(clinicId, date, ct);

                return items.Select(a => new AppointmentDto(a.Id, a.ClinicId, a.PatientId, a.Date, a.StartTime, a.EndTime));
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "");
              return Enumerable.Empty<AppointmentDto>();
            }
        }

        public async Task<AppointmentDto> BookAsync(CreateAppointmentRequest request, CancellationToken ct)
        {
            try
            {
                if (request.StartTime >= request.EndTime)
                    throw new ValidationException(new Dictionary<string, string[]>
                    { ["TimeRange"] = new[] { "Start must be before End." } });

                _logger.LogInformation("Creating appointment for patient {PatientId} at {Start}", request.PatientId, request.StartTime);

                var entity = await _repo.BookAsync(request.ClinicId, request.PatientId, request.Date, request.StartTime, request.EndTime, ct);
                return new AppointmentDto(entity.Id, entity.ClinicId, entity.PatientId, entity.Date, entity.StartTime, entity.EndTime);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation failed while booking appointment.");
                throw; // or return BadRequest(ex.Message);
            }
        }
    }
}

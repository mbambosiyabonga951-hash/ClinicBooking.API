using ClinicBooking.Application.DTOs;
using ClinicBooking.Application.Interfaces;
using ClinicBooking.Application.Providers;
using Microsoft.Extensions.Logging;

namespace ClinicBooking.Application.Services
{

    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;
        private readonly ITimeslotService _timeslotService;

        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(IAppointmentRepository repo, ITimeslotService timeslotService, ILogger<AppointmentService> log)
        {
            _repo = repo;
            _timeslotService = timeslotService;
            _logger = log;
        }
        public async Task<IEnumerable<AppointmentDto>> GetByClinicAndDateAsync(long clinicId, DateOnly date, CancellationToken ct)
        {
            try
            {
                var items = await _repo.GetByClinicAndDateAsync(clinicId, date, ct);

                return items.Select(a => new AppointmentDto(a.Id, a.ClinicId, a.PatientId, a.Date, a.StartUtc, a.EndUtc));
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
                if (request.StartUtc >= request.EndUtc)
                    throw new ValidationException(new Dictionary<string, string[]>
                    { ["TimeRange"] = new[] { "Start must be before End." } });

                _logger.LogInformation("Creating appointment for patient {PatientId} at {Start}", request.PatientId, request.StartUtc);

                var entity = await _repo.BookAsync(request.ClinicId, request.PatientId, request.Date, request.StartUtc, request.StartUtc, request.ProviderId, request.TimeslotId, ct);

                if(entity.Id > 0)
                    _logger.LogInformation("Successfully created appointment {AppointmentId} for patient {PatientId}", entity.Id, request.PatientId);
               var update = _timeslotService.UpdateTimeSlot(new UpdateTimeslotRequest
                {
                   TimeslotId = entity.TimeslotId,
                    IsBooked = true
                }, ct);

                if (update.Id > 0)
                    _logger.LogInformation("Successfully updated timeslot {TimeslotId} for appointment {AppointmentId}", entity.TimeslotId, entity.Id);
                else
                    _logger.LogWarning("Failed to update timeslot for appointment {AppointmentId}", entity.Id);

                return new AppointmentDto(entity.Id, entity.ClinicId, entity.PatientId, entity.Date, entity.StartUtc, entity.EndUtc);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation failed while booking appointment.");
                throw; // or return BadRequest(ex.Message);
            }
        }
    }
}

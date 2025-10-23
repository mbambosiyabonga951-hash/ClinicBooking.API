using ClinicBooking.Application.DTOs;

namespace ClinicBooking.Application.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDto>> GetByClinicAndDateAsync(long clinicId, DateOnly date, CancellationToken ct);
    Task<AppointmentDto> BookAsync(CreateAppointmentRequest request, CancellationToken ct);
}

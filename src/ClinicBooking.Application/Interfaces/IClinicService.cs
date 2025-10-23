using ClinicBooking.Application.DTOs;

namespace ClinicBooking.Application.Interfaces;

public interface IClinicService
{
    Task<IEnumerable<ClinicDto>> GetAllAsync(CancellationToken ct);
}

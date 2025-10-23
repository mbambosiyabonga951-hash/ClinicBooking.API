using ClinicBooking.Application.DTOs;

namespace ClinicBooking.Application.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetAllAsync(CancellationToken ct);
    Task<PatientDto> CreateAsync(CreatePatientRequest request, CancellationToken ct);
}

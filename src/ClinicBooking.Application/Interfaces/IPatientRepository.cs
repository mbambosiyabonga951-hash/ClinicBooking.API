using ClinicBooking.Domain.Entities;

namespace ClinicBooking.Application.Interfaces
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync(CancellationToken ct);
        Task<Patient> CreateAsync(string firstName, string lastName, string email, CancellationToken ct);

    }
}
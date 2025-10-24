using ClinicBooking.Domain.Entities;

namespace ClinicBooking.Application.Interfaces
{
    public interface ITokenService
    {
        string Create(ApplicationUser user, IEnumerable<string> roles);
    }
}

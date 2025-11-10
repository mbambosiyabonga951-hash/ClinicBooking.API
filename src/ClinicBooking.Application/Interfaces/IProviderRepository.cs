using ClinicBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBooking.Application.Interfaces
{
    public interface IProviderRepository
    {
        Task<IEnumerable<Provider>> GetAllAsync(CancellationToken ct);
        Task<Provider> CreateAsync(string FullName, string Specialty, CancellationToken ct);
    }
}

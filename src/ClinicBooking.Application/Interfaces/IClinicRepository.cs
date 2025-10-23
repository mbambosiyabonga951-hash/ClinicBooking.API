using ClinicBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBooking.Application.Interfaces
{
    public interface IClinicRepository
    {
        Task<IEnumerable<Clinic>> GetAllAsync(CancellationToken ct);
    }
}

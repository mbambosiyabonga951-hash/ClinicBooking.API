using ClinicBooking.Application.DTOs;
using ClinicBooking.Application.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBooking.Application.Interfaces
{
    public interface ITimeslotService
    {

        Task<IEnumerable<TimeslotDto>> GetByProviderAndDateAsync(int providerId, DateOnly date, CancellationToken ct = default);
        Task<TimeslotDto> CreateAsync(int providerId, DateTime startUtc, DateTime endUtc, CancellationToken ct = default);
    }
}

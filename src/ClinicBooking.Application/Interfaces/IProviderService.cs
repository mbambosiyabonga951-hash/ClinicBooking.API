using ClinicBooking.Application.DTOs;
using ClinicBooking.Application.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBooking.Application.Interfaces
{
    public interface IProviderService
    {
        Task<IEnumerable<ProviderDto>> GetAllAsync(CancellationToken ct);
        Task<ProviderDto> CreateAsync(UpsertProviderRequest request, CancellationToken ct);

    }
}

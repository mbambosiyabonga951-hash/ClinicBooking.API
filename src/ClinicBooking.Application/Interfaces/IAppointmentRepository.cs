using ClinicBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBooking.Application.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetByClinicAndDateAsync(long clinicId, DateOnly date, CancellationToken ct);
        Task<Appointment> BookAsync(long clinicId, long patientId, DateOnly date, TimeOnly start, TimeOnly end, long providerId, long timeslotId, CancellationToken ct);
    }
}

using ClinicBooking.Application.DTOs;
using ClinicBooking.Application.Requests;
using ClinicBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBooking.Application.Interfaces
{
    public interface ITimeslotRepository
    {

        Task<IEnumerable<Timeslot>> GetByProviderAndDateAsync(int providerId, DateOnly date, CancellationToken ct);
        Task<Timeslot> CreateAsync(int providerId, DateOnly BookingDate, TimeOnly startUtc, TimeOnly endUtc, CancellationToken ct);
        Task<Timeslot> UpdateTimeSlot(long id, bool isBooked, CancellationToken ct = default);



    }
}

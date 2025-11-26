using ClinicBooking.Application.Interfaces;
using ClinicBooking.Domain.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBooking.Infrastructure.Persistence.Dapper
{
    public class TimeslotRepository(ISqlConnectionFactory factory) : ITimeslotRepository
    {

        public async Task<IEnumerable<Timeslot>> GetByProviderAndDateAsync(int providerId, DateOnly date, CancellationToken ct)
        {
            try
            {
                using (var connection = factory.Create())
                {
                    var sql = @"
                            SELECT * FROM Timeslots
                             WHERE ProviderId = @ProviderId AND BookingDate = @Date AND IsBooked = 0";

                    var parameters = new
                    {
                        ProviderId = providerId,
                        Date = date.ToDateTime(TimeOnly.MinValue) // or .ToString("yyyy-MM-dd")
                    };

                    return await connection.QueryAsync<Timeslot>(sql, parameters);

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Something happened");
            }
        }

        public async Task<Timeslot> CreateAsync(int providerId, DateOnly bookingDate, TimeOnly startUtc, TimeOnly endUtc, CancellationToken ct)
        {
            try
            {
                using (var connection = factory.Create())
                {
                    var CreatedUtc = DateTime.UtcNow;

                    var parameters = new
                    {
                        ProviderId = providerId,
                        BookingDate = bookingDate.ToDateTime(TimeOnly.MinValue),
                        StartUtc = startUtc.ToTimeSpan(),
                        EndUtc = endUtc.ToTimeSpan(),
                        IsBooked = false,
                        CreatedUtc = DateTime.UtcNow,
                    };

                    var sql = @"
                        INSERT INTO Timeslots (ProviderId, BookingDate, StartUtc, EndUtc,IsBooked,CreatedUtc)
                        VALUES (@ProviderId, @BookingDate, @StartUtc, @EndUtc,0,@CreatedUtc);
                        SELECT * FROM Timeslots WHERE Id = SCOPE_IDENTITY();";

                    return await connection.QuerySingleAsync<Timeslot>(sql, parameters);

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Something wrong Happend");
            }
        }

        public async Task<Timeslot> UpdateTimeSlot(long id, bool isBooked, CancellationToken ct = default)
        {
            try
            {
                using (var connection = factory.Create())
                {
                    var result = await connection.QuerySingleOrDefaultAsync<Timeslot>(
                    "UPDATE Timeslots SET IsBooked = @IsBooked OUTPUT INSERTED.* WHERE Id = @Id",
                    new { Id = id, IsBooked = isBooked });

                    if (result == null)
                    {
                        // Handle case where no timeslot was updated (e.g., throw custom exception)
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update timeslot");
            }

        }
    }
}

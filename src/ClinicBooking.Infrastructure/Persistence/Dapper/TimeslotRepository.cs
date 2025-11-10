using ClinicBooking.Application.Interfaces;
using ClinicBooking.Domain.Entities;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBooking.Infrastructure.Persistence.Dapper
{
    public class TimeslotRepository(ISqlConnectionFactory factory) : ITimeslotRepository
    {
      
        public async Task<IEnumerable<Timeslot>> GetByProviderAndDateAsync(int providerId, DateOnly date, CancellationToken ct)
        {
            using var connection = factory.Create();
            var sql = @"SELECT Id, ProviderId, StartUtc, EndUtc, IsBooked 
                        FROM Timeslots 
                        WHERE ProviderId = @ProviderId AND IsBooked = 0
                        ORDER BY StartUtc ASC";
            return await connection.QueryAsync<Timeslot>(new CommandDefinition(sql, new { ProviderId = providerId }, cancellationToken: ct));
        }

        public async Task<Timeslot> CreateAsync(int providerId, DateTime startUtc, DateTime endUtc, CancellationToken ct)
        {
            using var connection = factory.Create();
            var sql = @"INSERT INTO Timeslots (ProviderId, StartUtc, EndUtc, IsBooked)
                        OUTPUT INSERTED.Id, INSERTED.ProviderId, INSERTED.StartUtc, INSERTED.EndUtc, INSERTED.IsBooked
                        VALUES (@ProviderId, @StartUtc, @EndUtc, 0);";
            return await connection.QuerySingleAsync<Timeslot>(new CommandDefinition(sql, new { ProviderId = providerId, StartUtc = startUtc, EndUtc = endUtc }, cancellationToken: ct));
        }

    }
}

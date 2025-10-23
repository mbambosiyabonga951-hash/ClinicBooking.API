using Dapper;
using ClinicBooking.Domain.Entities;
using ClinicBooking.Application.Interfaces;

namespace ClinicBooking.Infrastructure.Persistence.Dapper
{

    public class ClinicRepository(ISqlConnectionFactory factory) : IClinicRepository
    {
        public async Task<IEnumerable<Clinic>> GetAllAsync(CancellationToken ct)
        {
            try
            {
                using (var c = factory.Create())
                {
                    var sql = "SELECT Id, Name, Address FROM dbo.Clinics ORDER BY Name";
                    return await c.QueryAsync<Clinic>(new CommandDefinition(sql, cancellationToken: ct));
                }
            }
            catch (Exception ex)
            {
                // Log the exception (using NLog, Serilog, etc.)
                return Enumerable.Empty<Clinic>();
            }
        }
    }
}

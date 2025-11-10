using Dapper;
using ClinicBooking.Domain.Entities;
using ClinicBooking.Application.Interfaces;

namespace ClinicBooking.Infrastructure.Persistence.Dapper
{
    public class ProviderRepository(ISqlConnectionFactory factory) : IProviderRepository
    {
        public async Task<IEnumerable<Provider>> GetAllAsync(CancellationToken ct)
        {
            using var connection = factory.Create();
            var sql = "SELECT Id, FullName, Specialty FROM Providers ORDER BY Id DESC";
            return await connection.QueryAsync<Provider>(new CommandDefinition(sql, cancellationToken: ct));

        }

        public async Task<Provider> CreateAsync(string FullName, string Specialty, CancellationToken ct)
        {
            using var connection = factory.Create();

            var sql = @"INSERT INTO Providers (FirstName, LastName, Specialty)
                        OUTPUT INSERTED.Id, INSERTED.FirstName, INSERTED.LastName, INSERTED.Specialty
                        VALUES (@FullName, @Specialty);";

            return await connection.QuerySingleAsync<Provider>(new CommandDefinition(sql, new { FullName = FullName, Specialty = Specialty }, cancellationToken: ct));

        }   

    }
}

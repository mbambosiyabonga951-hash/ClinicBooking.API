using Dapper;
using ClinicBooking.Domain.Entities;
using ClinicBooking.Application.Interfaces;

namespace ClinicBooking.Infrastructure.Persistence.Dapper
{

    public class PatientRepository(ISqlConnectionFactory factory) : IPatientRepository
    {
        public async Task<IEnumerable<Patient>> GetAllAsync(CancellationToken ct)
        {
            using var c = factory.Create();
            var sql = "SELECT Id, FirstName, LastName, Email FROM dbo.Patients ORDER BY Id DESC";
            return await c.QueryAsync<Patient>(new CommandDefinition(sql, cancellationToken: ct));
        }

        public async Task<Patient> CreateAsync(string firstName, string lastName, string email, CancellationToken ct)
        {
            using var c = factory.Create();
            var sql = @"INSERT INTO dbo.Patients(FirstName, LastName, Email)
                    OUTPUT INSERTED.Id, INSERTED.FirstName, INSERTED.LastName, INSERTED.Email
                    VALUES(@FirstName, @LastName, @Email);";
            return await c.QuerySingleAsync<Patient>(new CommandDefinition(sql, new { FirstName = firstName, LastName = lastName, Email = email }, cancellationToken: ct));
        }
    }
}

using Dapper;
using ClinicBooking.Domain.Entities;
using ClinicBooking.Application.Interfaces;

namespace ClinicBooking.Infrastructure.Persistence.Dapper
{

    public class AppointmentRepository(ISqlConnectionFactory factory) : IAppointmentRepository
    {
        public async Task<IEnumerable<Appointment>> GetByClinicAndDateAsync(long clinicId, DateOnly date, CancellationToken ct)
        {
            using (var c = factory.Create())
            {
                var sql = @"SELECT Id, ClinicId, PatientId, Date, StartTime, EndTime
                    FROM dbo.Appointments
                    WHERE ClinicId = @ClinicId AND Date = @Date
                    ORDER BY StartTime";

                return await c.QueryAsync<Appointment>(new CommandDefinition(sql, new { ClinicId = clinicId, Date = date }, cancellationToken: ct));
            }
        }

        public async Task<Appointment> BookAsync(long clinicId, long patientId, DateOnly date, TimeOnly start, TimeOnly end, CancellationToken ct)
        {

            using (var c = factory.Create())
            {
                var sql = @"INSERT INTO dbo.Appointments(ClinicId, PatientId, Date, StartTime, EndTime)
                    OUTPUT INSERTED.Id, INSERTED.ClinicId, INSERTED.PatientId, INSERTED.Date, INSERTED.StartTime, INSERTED.EndTime
                    VALUES(@ClinicId, @PatientId, @Date, @StartTime, @EndTime)";

                return await c.QuerySingleAsync<Appointment>(new CommandDefinition(sql, new { ClinicId = clinicId, PatientId = patientId, Date = date, StartTime = start, EndTime = end }, cancellationToken: ct));
            }
        }
    }
}

using ClinicBooking.Application.Interfaces;
using ClinicBooking.Domain.Entities;
using Dapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        public async Task<Appointment> BookAsync(long clinicId, long patientId, DateOnly date, TimeOnly start, TimeOnly end, long providerId, long timeslotId, CancellationToken ct)
        {
            try
            {
                var parameters = new 
                {
                    ProviderId = providerId,
                    ClinicId = clinicId,
                    PatientId = patientId,
                    TimeSlotId = timeslotId,
                    Date = date,
                    StartUtc = start.ToTimeSpan(),
                    EndUtc = end.ToTimeSpan(),
                    CreatedUtc = DateTime.UtcNow

                };


                using (var c = factory.Create())
                {
                    var sql = @"INSERT INTO dbo.Appointments(ClinicId, PatientId, Date, StartUtc, EndUtc,ProviderId,Status,CreatedUtc,TimeslotId)
                    OUTPUT INSERTED.Id, INSERTED.ClinicId, INSERTED.PatientId, INSERTED.Date, INSERTED.StartUtc,
                    INSERTED.EndUtc, INSERTED.ProviderId,INSERTED.Status, INSERTED.CreatedUtc, INSERTED.TimeslotId
                    VALUES(@ClinicId, @PatientId, @Date, @StartUtc, @EndUtc,@ProviderId,0,@CreatedUtc,@TimeslotId)";

                    return await c.QuerySingleAsync<Appointment>(sql, parameters);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            }
    }
}

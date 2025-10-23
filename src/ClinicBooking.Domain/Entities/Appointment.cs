namespace ClinicBooking.Domain.Entities;

public class Appointment
{
    public long Id { get; set; }
    public long ClinicId { get; set; }
    public long PatientId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}

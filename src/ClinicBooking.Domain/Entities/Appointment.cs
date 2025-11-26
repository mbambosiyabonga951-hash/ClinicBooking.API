namespace ClinicBooking.Domain.Entities;

public class Appointment
{
    public long Id { get; set; }
    public long ClinicId { get; set; }
    public long PatientId { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartUtc { get; set; }
    public TimeOnly EndUtc { get; set; }
    public long ProviderId { get; set; }
    public int Status { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime LastUpdatedUtc { get; set; }
    public long TimeslotId { get; set; }    
}

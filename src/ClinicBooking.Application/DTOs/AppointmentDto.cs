namespace ClinicBooking.Application.DTOs;

public record AppointmentDto(long Id, long ClinicId, long PatientId, DateOnly Date, TimeOnly StartTime, TimeOnly EndTime);
public record CreateAppointmentRequest
{
    public long ClinicId { get; set; }
    public long ProviderId { get; set; }

    public long TimeslotId { get; set; }

    public DateOnly Date { get; set; }       // yyyy-MM-dd 

    public long PatientId { get; set; }
    public TimeOnly StartUtc { get; set; }
    public TimeOnly EndUtc { get; set; }
    public string? Notes { get; set; }
}

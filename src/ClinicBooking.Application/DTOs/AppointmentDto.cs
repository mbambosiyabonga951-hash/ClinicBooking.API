namespace ClinicBooking.Application.DTOs;

public record AppointmentDto(long Id, long ClinicId, long PatientId, DateOnly Date, TimeOnly StartTime, TimeOnly EndTime);
public record CreateAppointmentRequest(long ClinicId, long PatientId, DateOnly Date, TimeOnly StartTime, TimeOnly EndTime);

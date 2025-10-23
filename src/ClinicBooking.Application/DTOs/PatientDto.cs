namespace ClinicBooking.Application.DTOs;

public record PatientDto(long Id, string FirstName, string LastName, string Email);
public record CreatePatientRequest(string FirstName, string LastName, string Email);

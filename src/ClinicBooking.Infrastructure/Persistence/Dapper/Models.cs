using ClinicBooking.Domain.Entities;

namespace ClinicBooking.Infrastructure.Persistence.Dapper;

public static class Mappers
{
    public static Patient ToPatient(this Patient row) => row;
    public static Clinic ToClinic(this Clinic row) => row;
    public static Appointment ToAppointment(this Appointment row) => row;
}

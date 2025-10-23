using Microsoft.EntityFrameworkCore;
using ClinicBooking.Domain.Entities;

namespace ClinicBooking.Infrastructure.Persistence;

public class ClinicBookingDbContext(DbContextOptions<ClinicBookingDbContext> options) : DbContext(options)
{
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Clinic> Clinics => Set<Clinic>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>().HasKey(x => x.Id);
        modelBuilder.Entity<Clinic>().HasKey(x => x.Id);
        modelBuilder.Entity<Appointment>().HasKey(x => x.Id);

        modelBuilder.Entity<Appointment>()
            .HasIndex(a => new { a.ClinicId, a.Date });
    }
}

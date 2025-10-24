using ClinicBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicBooking.Infrastructure.Persistence
{
    public class ClinicBookingDbContext : DbContext
    {
        public ClinicBookingDbContext(DbContextOptions<ClinicBookingDbContext> options)
            : base(options) { }

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
}
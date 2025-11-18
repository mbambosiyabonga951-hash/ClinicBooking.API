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
        public DbSet<Provider> Providers => Set<Provider>();
        public DbSet<Timeslot> Timeslots => Set<Timeslot>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>().HasKey(x => x.Id);
            modelBuilder.Entity<Clinic>().HasKey(x => x.Id);
            modelBuilder.Entity<Appointment>().HasKey(x => x.Id);
            modelBuilder.Entity<Timeslot>().HasKey(x => x.Id);  
            modelBuilder.Entity<Provider>().HasKey(x => x.Id);

            // Provider configuration
            modelBuilder.Entity<Provider>(b =>
            {
                b.ToTable("Providers");
                b.HasKey(x => x.Id);

                b.Property(x => x.Name).IsRequired().HasMaxLength(200);
                b.Property(x => x.Specialty).HasMaxLength(100);

                b.HasOne(x => x.Clinic)
                 .WithMany(c => c.Providers)
                 .HasForeignKey(x => x.ClinicId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // Timeslot configuration
            modelBuilder.Entity<Timeslot>(b =>
            {
                b.ToTable("Timeslots");
                b.HasKey(x => x.Id);

                b.Property(x => x.StartUtc).IsRequired();
                b.Property(x => x.EndUtc).IsRequired();
                b.Property(x => x.IsBooked).HasDefaultValue(false);

                b.HasOne(x => x.Provider)
                 .WithMany(p => p.Timeslots)
                 .HasForeignKey(x => x.ProviderId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
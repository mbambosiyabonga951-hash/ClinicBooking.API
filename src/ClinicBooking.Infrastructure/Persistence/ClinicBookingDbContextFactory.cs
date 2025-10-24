using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ClinicBooking.Infrastructure.Persistence
{
    public class ClinicBookingDbContextFactory : IDesignTimeDbContextFactory<ClinicBookingDbContext>
    {
        public ClinicBookingDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ClinicBookingDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("ClinicDb"));

            return new ClinicBookingDbContext(optionsBuilder.Options);
        }
    }
}
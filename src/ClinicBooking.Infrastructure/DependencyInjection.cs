using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ClinicBooking.Infrastructure.Persistence;
using ClinicBooking.Infrastructure.Persistence.Dapper;
using ClinicBooking.Application.Services;
using ClinicBooking.Application.Interfaces;

namespace ClinicBooking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core context already added in API (Program.cs) - left here if moving to worker/API agnostic composition
        // Dapper factory
        var cs = configuration.GetConnectionString("DefaultConnection") ?? "";
        services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(cs));

        // Repositories
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IClinicRepository, ClinicRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();

        // Application services wiring (keeps API Program.cs clean)
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<IClinicService, ClinicService>();
        services.AddScoped<ITimeslotService, TimeslotService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IProviderService, ProviderService>();
        services.AddScoped<IProviderRepository, ProviderRepository>();
        services.AddScoped<ITimeslotRepository, TimeslotRepository>();

        return services;
    }
}

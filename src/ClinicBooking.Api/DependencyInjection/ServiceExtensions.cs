using Microsoft.Extensions.DependencyInjection;

namespace ClinicBooking.Api.DependencyInjection;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        => services; // reserved for API-level services (filters, mappers)
}

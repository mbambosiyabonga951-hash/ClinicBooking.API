using ClinicBooking.Api.Common;
using ClinicBooking.Api.DependencyInjection;
using ClinicBooking.Application.Helpers;
using ClinicBooking.Infrastructure;
using ClinicBooking.Infrastructure.Persistence;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using static ClinicBooking.Application.Helpers.DateOnlyHandler;



var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Info("Starting ClinicBooking API...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Read log directory from configuration
    var logDir = builder.Configuration["Logging:LogDirectory:Path"]
                 ?? Path.Combine(AppContext.BaseDirectory, "logs");

    // Ensure directory exists
    Directory.CreateDirectory(logDir);

    // Override NLog variable with appsettings value
    LogManager.Configuration.Variables["logDir"] = logDir;
    LogManager.ReconfigExistingLoggers();

    logger.Info($"Logging directory configured at: {logDir}");

    // NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddTransient<CorrelationIdMiddleware>();


    // Controllers & Swagger
    //builder.Services.AddControllers();
    builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // DbContext (EF Core)
    builder.Services.AddDbContext<ClinicBookingDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Dapper Type Handlers
    SqlMapper.AddTypeHandler(new DateOnlyHandler());
    SqlMapper.AddTypeHandler(new TimeOnlyHandler());

    // Application + Infrastructure
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();
    app.UseMiddleware<CorrelationIdMiddleware>();
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}

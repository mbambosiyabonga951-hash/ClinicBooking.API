using ClinicBooking.Api.Common;
using ClinicBooking.Api.DependencyInjection;
using ClinicBooking.Application.Helpers;
using ClinicBooking.Application.Interfaces;
using ClinicBooking.Domain.Entities;
using ClinicBooking.Infrastructure;
using ClinicBooking.Infrastructure.Persistence;
using ClinicBooking.Infrastructure.Services;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using System;
using System.Text;
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

    //// DbContext (EF Core)
    //builder.Services.AddDbContext<ClinicBookingDbContext>(opt =>
    //    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddDbContext<ClinicBookingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ClinicDb")));

    builder.Services.AddDbContext<AppIdentityDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDb")));

    builder.Services.AddIdentity<ApplicationUser, IdentityRole<long>>()
        .AddEntityFrameworkStores<AppIdentityDbContext>()
        .AddDefaultTokenProviders();


    // EF
    //builder.Services.AddDbContext<AppDbContext>(opt =>
    //    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    //// Or: opt.UseNpgsql(...);

    // Identity
    builder.Services
        .AddIdentityCore<ApplicationUser>(opt =>
        {
            opt.Password.RequireDigit = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequiredLength = 6;
        })
        .AddRoles<IdentityRole<long>>()
        .AddEntityFrameworkStores<ClinicBookingDbContext>()
        .AddSignInManager<SignInManager<ApplicationUser>>();

    // JWT Auth
    //var jwt = builder.Configuration.GetSection("Jwt");
    //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
    var jwt = builder.Configuration.GetSection("Jwt");
    var keyString = jwt["Key"];
    if (string.IsNullOrWhiteSpace(keyString))
        throw new InvalidOperationException("Configuration missing: 'Jwt:Key'. Add it to appsettings.json, environment variables, or user secrets.");

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new()
            {
                ValidIssuer = jwt["Issuer"],
                ValidAudience = jwt["Audience"],
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.FromSeconds(30)
            };
        });

    builder.Services.AddAuthorization();

    // CORS for the WASM origin
    builder.Services.AddCors(o => o.AddPolicy("spa", p =>
        p.WithOrigins("https://localhost:58389", "http://localhost:5026")
         .AllowAnyHeader().AllowAnyMethod()));

    // Token service
    builder.Services.AddScoped<ITokenService, TokenService>();

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
    app.UseCors(builder =>
    builder.WithOrigins("https://localhost:58389") // or your actual client URL
           .AllowAnyHeader()
           .AllowAnyMethod());

    app.UseAuthentication();
    app.UseAuthorization();
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

using ClinicBooking.Application.DTOs;
using ClinicBooking.Application.Interfaces;
using ClinicBooking.Application.Requests;
using Microsoft.Extensions.Logging;
namespace ClinicBooking.Application.Services
{
    public class TimeslotService : ITimeslotService
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ITimeslotRepository _timeslotRepository;

        public TimeslotService(ILoggerFactory loggerFactory, ITimeslotRepository timeslotRepository)
        {
            _loggerFactory = loggerFactory;
            _timeslotRepository = timeslotRepository;
        }

        public async Task<IEnumerable<TimeslotDto>> GetByProviderAndDateAsync(int providerId, DateOnly date, CancellationToken ct = default)
        {
            var logger = _loggerFactory.CreateLogger<TimeslotService>();
            logger.LogInformation("Fetching available timeslots for ProviderId: {ProviderId} on {Date}",
                providerId, date);
            var timeslots = await _timeslotRepository.GetByProviderAndDateAsync(providerId, date, ct);
            logger.LogInformation("Found {Count} available timeslots for ProviderId: {ProviderId}", timeslots.Count(), providerId   );
            return (IEnumerable<TimeslotDto>)timeslots;
        }

        public async Task<TimeslotDto> CreateAsync(int providerId, DateTime startUtc, DateTime endUtc, CancellationToken ct = default)
        {
            var logger = _loggerFactory.CreateLogger<TimeslotService>();
            logger.LogInformation("Creating timeslot for ProviderId: {ProviderId} between {StartDate} and {EndDate}",
                providerId, startUtc, endUtc);

            var timeslot = await _timeslotRepository.CreateAsync(providerId, startUtc, endUtc, ct);



            logger.LogInformation("Created timeslot with Id: {TimeslotId} for ProviderId: {ProviderId}", timeslot.Id, providerId);


            return  new TimeslotDto
            {
                Id = timeslot.Id,
                ProviderId = providerId,
                StartTime = startUtc,
                EndTime = endUtc
            };
        }
    }
}
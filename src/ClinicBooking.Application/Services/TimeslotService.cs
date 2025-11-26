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
            try
            {
                var logger = _loggerFactory.CreateLogger<TimeslotService>();
                logger.LogInformation("Fetching available timeslots for ProviderId: {ProviderId} on {Date}",
                    providerId, date);
                var timeslots = await _timeslotRepository.GetByProviderAndDateAsync(providerId, date, ct);
                logger.LogInformation("Found {Count} available timeslots for ProviderId: {ProviderId}", timeslots.Count(), providerId);

                return timeslots.Select(t => new TimeslotDto
                {
                    Id = t.Id,
                    ProviderId = t.ProviderId,
                    StartTime = t.StartUtc,
                    EndTime = t.EndUtc
                });
            }
            catch (Exception ex)
            {
                // Return an empty list if an exception occurs
                return Enumerable.Empty<TimeslotDto>();
            }
        }

        public async Task<TimeslotDto> CreateAsync(int providerId, DateOnly BookingDate, TimeOnly startUtc, TimeOnly endUtc, CancellationToken ct = default)
        {
            var logger = _loggerFactory.CreateLogger<TimeslotService>();
            logger.LogInformation("Creating timeslot for ProviderId: {ProviderId} between {StartDate} and {EndDate}",
                providerId, startUtc, endUtc);

            var timeslot = await _timeslotRepository.CreateAsync(providerId, BookingDate, startUtc, endUtc, ct);



            logger.LogInformation("Created timeslot with Id: {TimeslotId} for ProviderId: {ProviderId}", timeslot.Id, providerId);


            return  new TimeslotDto
            {
                Id = timeslot.Id,
                ProviderId = providerId,
                StartTime = startUtc,
                EndTime = endUtc
            };
        }

        public async Task<TimeslotDto> UpdateTimeSlot(UpdateTimeslotRequest request, CancellationToken ct)
        { 
            var logger = _loggerFactory.CreateLogger<TimeslotService>();

            logger.LogInformation("Updating timeslot Id: {TimeslotId} to new times {StartUtc} - {EndUtc}",
                request.TimeslotId, request.StartUtc, request.EndUtc);

            var updatedTimeslot = await _timeslotRepository.UpdateTimeSlot(request.TimeslotId,request.IsBooked, ct);
            logger.LogInformation("Updated timeslot Id: {TimeslotId}", updatedTimeslot.Id);
            return new TimeslotDto
            {
                Id = updatedTimeslot.Id,
                ProviderId = updatedTimeslot.ProviderId,
                StartTime = updatedTimeslot.StartUtc,
                EndTime = updatedTimeslot.EndUtc
            };

        }
    }
}
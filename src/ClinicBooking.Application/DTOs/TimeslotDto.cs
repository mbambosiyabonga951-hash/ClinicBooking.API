using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBooking.Application.DTOs
{
    public class TimeslotDto
    {
        public long Id { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public long ProviderId { get; set; }
    }

    public class CreateTimeslotDto
    {
        public int ProviderId { get; set; }
        public DateOnly BookingDate { get; set; }
        public TimeOnly StartUtc { get; set; }
        public TimeOnly EndUtc { get; set; }
    }

    public class UpdateTimeslotRequest
    {
        public long TimeslotId { get; set; }
        public bool IsBooked { get; set; }
        public long ProviderId { get; set; }
        public TimeOnly StartUtc { get; set; }
        public TimeOnly EndUtc { get; set; }


    }
}

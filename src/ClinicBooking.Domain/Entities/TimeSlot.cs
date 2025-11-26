using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBooking.Domain.Entities
{
    public class Timeslot 
    {
        public long Id { get; set; }
        public long ProviderId { get; set; }
        public Provider? Provider { get; set; }
        public DateOnly BookingDate { get; set; }
        public TimeOnly StartUtc { get; set; }
        public TimeOnly EndUtc { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public bool IsBooked { get; set; } = false;
    }
}

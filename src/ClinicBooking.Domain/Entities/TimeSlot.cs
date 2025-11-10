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
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public bool IsBooked { get; set; } = false;
    }
}

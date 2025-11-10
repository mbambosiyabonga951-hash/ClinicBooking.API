using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBooking.Application.Requests
{
    public record CreateTimeslotRequest
    {

        public long ProviderId { get; set; }

       public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }

    }
}

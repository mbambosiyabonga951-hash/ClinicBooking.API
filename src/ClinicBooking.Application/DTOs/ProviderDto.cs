using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBooking.Application.DTOs
{
    public class ProviderDto
    {
        public long Id { get; set; }
        public string FullName { get; set; } = "";
        public string Specialty { get; set; } = "";
        public long ClinicId { get; set; }
    }
}

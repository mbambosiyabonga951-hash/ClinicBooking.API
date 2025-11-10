using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBooking.Domain.Entities
{
    public class Provider 
    {
        public long Id { get; set; }
        public string FullName { get; set; } = "";
        public string Specialty { get; set; } = "";
        public long ClinicId { get; set; }
        public Clinic? Clinic { get; set; }
        public ICollection<Timeslot> Timeslots { get; set; } = new List<Timeslot>();
    }
}

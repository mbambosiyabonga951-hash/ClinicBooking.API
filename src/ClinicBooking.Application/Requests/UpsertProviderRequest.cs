using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBooking.Application.Requests
{
    public class UpsertProviderRequest
    {
        //public UpsertProviderRequest() { }
        //public UpsertProviderRequest(string name, string specialty, long clinicId)
        //{
        //    Name = name;
        //    Specialty = specialty;
        //    ClinicId = clinicId;
        //}
        public long Id { get; set; }
        public string FullName { get; set; } = "";
        public string Specialty { get; set; } = "";
        public long ClinicId { get; set; }
    }
}

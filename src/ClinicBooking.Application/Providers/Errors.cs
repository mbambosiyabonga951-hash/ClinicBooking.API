using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBooking.Application.Providers
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entity, object key)
            : base($"{entity} with key '{key}' was not found.") { }
    }

    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }

    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }
        public ValidationException(IDictionary<string, string[]> errors)
            : base("One or more validation errors occurred.") => Errors = errors;
    }

}

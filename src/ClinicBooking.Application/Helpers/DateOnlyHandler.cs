using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBooking.Application.Helpers
{
    public class DateOnlyHandler : SqlMapper.TypeHandler<DateOnly>
    {
        public override void SetValue(IDbDataParameter parameter, DateOnly value)
        {
            parameter.Value = value.ToDateTime(TimeOnly.MinValue);
            parameter.DbType = DbType.Date;
        }

        public override DateOnly Parse(object value)
        {
            return DateOnly.FromDateTime((DateTime)value);
        }

        public class TimeOnlyHandler : SqlMapper.TypeHandler<TimeOnly>
        {
            public override void SetValue(IDbDataParameter parameter, TimeOnly value)
            {
                parameter.Value = value.ToTimeSpan();
                parameter.DbType = DbType.Time;
            }

            public override TimeOnly Parse(object value)
            {
                return TimeOnly.FromTimeSpan((TimeSpan)value);
            }
        }
    }
}

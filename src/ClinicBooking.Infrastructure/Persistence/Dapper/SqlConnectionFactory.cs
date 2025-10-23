using Microsoft.Data.SqlClient;
using System.Data;

namespace ClinicBooking.Infrastructure.Persistence.Dapper
{

    public interface ISqlConnectionFactory
    {
        IDbConnection Create();
    }

    public class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
    {
        public IDbConnection Create() => new SqlConnection(connectionString);
    }
}

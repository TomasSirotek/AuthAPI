using System.Data;
using Microsoft.Data.SqlClient;

namespace ProductAPI.Infrastructure.Data {
    public class SqlServerConnection {
        private readonly string _connectionString;

        public SqlServerConnection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnector");
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
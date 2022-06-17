using System.Data;
using Microsoft.Data.SqlClient;

namespace ProductAPI.Infrastructure.Data {
    public class SqlServerConnection {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public SqlServerConnection(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnector");
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
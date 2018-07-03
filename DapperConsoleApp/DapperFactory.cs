using Devart.Data.Oracle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DapperConsoleApp
{
    public class DapperFactory
    {
        public static readonly string connectionString = "Data Source=127.0.0.1/orcl;User ID=tjpis;Password=tjpis";
        public static OracleConnection CrateOracleConnection()
        {
            var connection = new OracleConnection(connectionString);
            connection.Open();
            return connection;
        }

    }
}

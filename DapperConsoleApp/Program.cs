using Dapper;
using Devart.Data.Oracle;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DapperConsoleApp
{
    class Program
    {
        //Devart数据库相关
        private static int DevartOracleClient()
        {
            string ConnStr = "Data Source=127.0.0.1/orcl;User ID=tjpis;Password=tjpis";
            int num = 0;
            using (Devart.Data.Oracle.OracleConnection oracleConnection = new Devart.Data.Oracle.OracleConnection(ConnStr))
            {
                Devart.Data.Oracle.OracleCommand oracleCommand = oracleConnection.CreateCommand();
                oracleCommand.CommandText = "select * from exam_master";
                oracleConnection.Open();
                using (Devart.Data.Oracle.OracleDataReader oracleDataReader = oracleCommand.ExecuteReader())
                {
                    while (oracleDataReader.Read())
                    {
                        object[] values = new object[500];
                        oracleDataReader.GetValues(values);
                        num++;
                    }
                }
                return num;
            }
        }
        static void Main(string[] args)
        {
            using (OracleConnection conn = DapperFactory.CrateOracleConnection())
            {
                String executeSql = @"SELECT * FROM FTP_SET_INFO WHERE ID = :ID";
                var conditon = new { ID = 1 };
                var query = conn.Query<FTP_SET_INFO>(executeSql, conditon).SingleOrDefault();
               Console.WriteLine(JsonConvert.SerializeObject(query));
            }
            Console.Read();
        }
    }
}

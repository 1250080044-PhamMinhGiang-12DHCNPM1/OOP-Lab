using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyHoSoBenhAn.Data
{
    public static class Db
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["QuanLyHoSoBenhAnDb"].ConnectionString;

        public static SqlConnection OpenConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        public static DataTable Query(string sql, params SqlParameter[] parameters)
        {
            using (var connection = OpenConnection())
            using (var command = new SqlCommand(sql, connection))
            using (var adapter = new SqlDataAdapter(command))
            {
                if (parameters != null) command.Parameters.AddRange(parameters);
                var table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        public static int Execute(string sql, params SqlParameter[] parameters)
        {
            using (var connection = OpenConnection())
            using (var command = new SqlCommand(sql, connection))
            {
                if (parameters != null) command.Parameters.AddRange(parameters);
                return command.ExecuteNonQuery();
            }
        }
    }
}

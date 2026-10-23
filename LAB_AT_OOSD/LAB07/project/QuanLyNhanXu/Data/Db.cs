using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyNhanXu.Data
{
    public static class Db
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["QuanLyNhanSuDongA"].ConnectionString;

        public static SqlConnection OpenConnection()
        {
            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
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

        public static object Procedure(string name, string outputName, SqlDbType outputType, int outputSize, params SqlParameter[] parameters)
        {
            using (var connection = OpenConnection())
            using (var command = new SqlCommand(name, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                if (parameters != null) command.Parameters.AddRange(parameters);
                SqlParameter output = null;
                if (!string.IsNullOrWhiteSpace(outputName))
                {
                    output = new SqlParameter(outputName, outputType, outputSize) { Direction = ParameterDirection.Output };
                    command.Parameters.Add(output);
                }
                command.ExecuteNonQuery();
                return output == null ? null : output.Value;
            }
        }
    }
}

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyCongTyDuLich.Data
{
    public static class Db
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["QuanLyCongTyDuLich"].ConnectionString;

        public static SqlConnection Open()
        {
            var cn = new SqlConnection(ConnectionString);
            cn.Open();
            return cn;
        }

        public static DataTable Query(string sql, params SqlParameter[] parameters)
        {
            using (var cn = Open())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddRange(parameters);
                using (var da = new SqlDataAdapter(cmd))
                {
                    var table = new DataTable();
                    da.Fill(table);
                    return table;
                }
            }
        }

        public static int Execute(string sql, params SqlParameter[] parameters)
        {
            using (var cn = Open())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }

        public static object Scalar(string sql, params SqlParameter[] parameters)
        {
            using (var cn = Open())
            using (var cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
            }
        }
    }
}

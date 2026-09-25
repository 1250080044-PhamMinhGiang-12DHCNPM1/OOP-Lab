using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyKhachSan.Data
{
    public static class Db
    {
        public static string ConnectionString { get { return ConfigurationManager.ConnectionStrings["QuanLyKhachSanDB"].ConnectionString; } }
        public static SqlConnection Open()
        {
            var connection = new SqlConnection(ConnectionString); connection.Open(); return connection;
        }
        public static SqlParameter P(string name, object value) { return new SqlParameter(name, value ?? DBNull.Value); }
        public static DataTable Query(string sql, params SqlParameter[] parameters)
        {
            using (var cn = Open()) using (var cmd = new SqlCommand(sql, cn)) using (var adapter = new SqlDataAdapter(cmd))
            { if (parameters != null) cmd.Parameters.AddRange(parameters); var table = new DataTable(); adapter.Fill(table); return table; }
        }
        public static int Execute(string sql, params SqlParameter[] parameters)
        {
            using (var cn = Open()) using (var cmd = new SqlCommand(sql, cn))
            { if (parameters != null) cmd.Parameters.AddRange(parameters); return cmd.ExecuteNonQuery(); }
        }
        public static object Scalar(string sql, params SqlParameter[] parameters)
        {
            using (var cn = Open()) using (var cmd = new SqlCommand(sql, cn))
            { if (parameters != null) cmd.Parameters.AddRange(parameters); return cmd.ExecuteScalar(); }
        }
        public static void Transaction(Action<SqlConnection, SqlTransaction> action)
        {
            using (var cn = Open()) using (var tx = cn.BeginTransaction())
            { try { action(cn, tx); tx.Commit(); } catch { tx.Rollback(); throw; } }
        }
        public static int Execute(SqlConnection cn, SqlTransaction tx, string sql, params SqlParameter[] parameters)
        {
            using (var cmd = new SqlCommand(sql, cn, tx)) { if (parameters != null) cmd.Parameters.AddRange(parameters); return cmd.ExecuteNonQuery(); }
        }
        public static object Scalar(SqlConnection cn, SqlTransaction tx, string sql, params SqlParameter[] parameters)
        {
            using (var cmd = new SqlCommand(sql, cn, tx)) { if (parameters != null) cmd.Parameters.AddRange(parameters); return cmd.ExecuteScalar(); }
        }
    }
}

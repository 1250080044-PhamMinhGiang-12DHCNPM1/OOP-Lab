using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Test_giaodien.Data
{
    public static class Db
    {
        private static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["QuanLyThuVienDb"].ConnectionString; }
        }

        public static DataTable Query(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                if (parameters != null && parameters.Length > 0)
                {
                    command.Parameters.AddRange(parameters);
                }

                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }
    }
}

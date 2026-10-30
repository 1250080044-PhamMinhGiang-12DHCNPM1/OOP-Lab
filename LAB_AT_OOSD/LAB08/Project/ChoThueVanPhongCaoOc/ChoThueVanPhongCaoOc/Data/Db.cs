using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ChoThueVanPhongCaoOc.Data
{
    internal static class Db
    {
        internal static SqlConnection Open()
        {
            var cn = new SqlConnection(ConfigurationManager.ConnectionStrings["QLChoThueVanPhong"].ConnectionString);
            cn.Open();
            return cn;
        }

        internal static DataTable Query(string sql, params SqlParameter[] parameters)
        {
            using (var cn = Open())
            using (var cmd = new SqlCommand(sql, cn))
            using (var da = new SqlDataAdapter(cmd))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                var table = new DataTable();
                da.Fill(table);
                return table;
            }
        }

        internal static int Execute(string sql, params SqlParameter[] parameters)
        {
            using (var cn = Open())
            using (var cmd = new SqlCommand(sql, cn))
            {
                if (parameters != null) cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }
    }
}

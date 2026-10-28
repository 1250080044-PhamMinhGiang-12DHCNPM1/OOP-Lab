using System.Configuration;
using System.Data.SqlClient;

namespace WindowsFormsApp1.Data
{
    public static class KetNoiDuLieu
    {
        public static SqlConnection TaoKetNoi()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["EShoppingDb"].ConnectionString);
        }
    }
}

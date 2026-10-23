using System.Data;
using System.Data.SqlClient;
using QuanLyNhanXu.Data;

namespace QuanLyNhanXu.Services
{
    public sealed class BangLuongService
    {
        public void TinhLuongThang(int thang, int nam)
        {
            Db.Procedure("dbo.sp_TinhLuongThang", null, SqlDbType.Int, 0, new SqlParameter("@Thang", thang), new SqlParameter("@Nam", nam));
        }

        public void ChotBangLuong(int thang, int nam)
        {
            Db.Procedure("dbo.sp_ChotBangLuong", null, SqlDbType.Int, 0, new SqlParameter("@Thang", thang), new SqlParameter("@Nam", nam));
        }

        public DataTable LayChiTiet(int thang, int nam)
        {
            return Db.Query(@"SELECT c.MaNV,n.HoTen,c.SoNgayCong,c.LuongCoBan,c.LuongTheoCong,c.TongPhuCap,c.SoGioPhuTroi,c.TienPhuTroi,c.TienThuong,c.TienPhat,c.KhauTruBHXH,c.KhauTruBHYT,c.ThucLinh,b.TrangThai FROM ChiTietBangLuong c JOIN BangLuong b ON b.MaBangLuong=c.MaBangLuong JOIN NhanVien n ON n.MaNV=c.MaNV WHERE b.Thang=@t AND b.Nam=@n", new SqlParameter("@t", thang), new SqlParameter("@n", nam));
        }
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyNhanXu.Data;
using QuanLyNhanXu.Models;

namespace QuanLyNhanXu.Services
{
    public sealed class HopDongBaoHiemService
    {
        public int KyHopDongChinhThuc(HopDongInput input)
        {
            if (input == null || string.IsNullOrWhiteSpace(input.MaNV)) throw new ArgumentException("Nhập mã nhân viên.");
            return Convert.ToInt32(Db.Procedure("dbo.sp_KyHopDongChinhThuc", "@MaHD", SqlDbType.Int, 0,
                new SqlParameter("@MaNV", input.MaNV.Trim()), new SqlParameter("@LoaiHopDong", input.LoaiHopDong),
                new SqlParameter("@NgayKy", input.NgayKy.Date), new SqlParameter("@TuNgay", input.TuNgay.Date),
                new SqlParameter("@DenNgay", (object)(input.DenNgay.HasValue ? input.DenNgay.Value.Date : (DateTime?)null) ?? DBNull.Value)));
        }

        public void TaoHoSoBaoHiem(string maNV, string soBHXH, string soBHYT, DateTime tuNgay)
        {
            Db.Procedure("dbo.sp_TaoHoSoBaoHiem", null, SqlDbType.Int, 0,
                new SqlParameter("@MaNV", maNV.Trim()), new SqlParameter("@SoBHXH", soBHXH.Trim()),
                new SqlParameter("@SoBHYT", soBHYT.Trim()), new SqlParameter("@TuNgay", tuNgay.Date));
        }

        public DataTable DanhSachHopDong()
        {
            return Db.Query("SELECT h.MaHD,h.MaNV,n.HoTen,h.LoaiHopDong,h.TuNgay,h.DenNgay,h.TrangThai,b.SoBHXH,b.SoBHYT FROM HopDongLaoDong h JOIN NhanVien n ON n.MaNV=h.MaNV LEFT JOIN BaoHiem b ON b.MaHD=h.MaHD ORDER BY h.MaHD DESC");
        }
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyNhanXu.Data;
using QuanLyNhanXu.Models;

namespace QuanLyNhanXu.Services
{
    public sealed class TuyenDungService
    {
        public string TiepNhanThuViec(ThuViecInput input)
        {
            if (input == null || string.IsNullOrWhiteSpace(input.HoTen) || string.IsNullOrWhiteSpace(input.CCCD))
                throw new ArgumentException("Họ tên và CCCD là bắt buộc.");
            if (input.DenNgayThuViec.Date < input.TuNgay.Date || input.LuongThuViec <= 0)
                throw new ArgumentException("Thời gian hoặc lương thử việc không hợp lệ.");
            return Convert.ToString(Db.Procedure("dbo.sp_TiepNhanThuViec", "@MaNV", SqlDbType.VarChar, 10,
                new SqlParameter("@HoTen", input.HoTen.Trim()), new SqlParameter("@NgaySinh", input.NgaySinh.Date),
                new SqlParameter("@GioiTinh", input.GioiTinh ?? "Khác"), new SqlParameter("@CCCD", input.CCCD.Trim()),
                new SqlParameter("@DienThoai", (object)input.DienThoai ?? DBNull.Value), new SqlParameter("@DiaChi", (object)input.DiaChi ?? DBNull.Value),
                new SqlParameter("@MaPhongBan", input.MaPhongBan), new SqlParameter("@MaNhuCau", (object)input.MaNhuCau ?? DBNull.Value),
                new SqlParameter("@TuNgay", input.TuNgay.Date), new SqlParameter("@DenNgayThuViec", input.DenNgayThuViec.Date),
                new SqlParameter("@LuongThuViec", input.LuongThuViec), new SqlParameter("@DieuKhoan", (object)input.DieuKhoan ?? DBNull.Value)));
        }

        public void DanhGiaThuViec(string maNV, bool dat)
        {
            Db.Procedure("dbo.sp_DanhGiaThuViec", null, SqlDbType.Int, 0,
                new SqlParameter("@MaNV", maNV), new SqlParameter("@KetQua", dat ? "Dat" : "Khong dat"));
        }

        public DataTable DanhSachNhanVien()
        {
            return Db.Query("SELECT n.MaNV,n.HoTen,n.CCCD,p.TenPhongBan,n.TrangThai FROM NhanVien n LEFT JOIN PhongBan p ON p.MaPhongBan=n.MaPhongBan ORDER BY n.MaNV DESC");
        }
    }
}

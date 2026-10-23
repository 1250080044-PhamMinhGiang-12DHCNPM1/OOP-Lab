using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyNhanXu.Data;

namespace QuanLyNhanXu.Services
{
    public sealed class QuyetDinhService
    {
        public void BanHanhQuyetDinhLuong(string maNV, DateTime hieuLuc, decimal luongCoBan, decimal phuCap, decimal docHai)
        {
            if (luongCoBan <= 0 || phuCap < 0 || docHai < 0) throw new ArgumentException("Các khoản lương không hợp lệ.");
            Db.Procedure("dbo.sp_BanHanhQuyetDinhLuong", null, SqlDbType.Int, 0,
                new SqlParameter("@MaNV", maNV.Trim()), new SqlParameter("@NgayHieuLuc", hieuLuc.Date),
                new SqlParameter("@LuongCoBan", luongCoBan), new SqlParameter("@PhuCapLuong", phuCap), new SqlParameter("@PhuCapDocHai", docHai));
        }

        public void BanHanhQuyetDinhChucVu(string maNV, int maChucVu, DateTime hieuLuc, decimal phuCap)
        {
            Db.Procedure("dbo.sp_BanHanhQuyetDinhChucVu", null, SqlDbType.Int, 0,
                new SqlParameter("@MaNV", maNV.Trim()), new SqlParameter("@MaChucVu", maChucVu),
                new SqlParameter("@NgayHieuLuc", hieuLuc.Date), new SqlParameter("@PhuCapChucVu", phuCap));
        }

        public DataTable LichSu(string maNV)
        {
            const string sql = @"SELECT N'Lương' Loai,NgayHieuLuc,LuongCoBan GiaTri,PhuCapLuong+PhuCapDocHai PhuCap FROM QuyetDinhLuong WHERE MaNV=@m
UNION ALL SELECT N'Chức vụ',q.NgayHieuLuc,CAST(q.MaChucVu AS DECIMAL(18,2)),q.PhuCapChucVu FROM QuyetDinhChucVu q WHERE q.MaNV=@m ORDER BY NgayHieuLuc DESC";
            return Db.Query(sql, new SqlParameter("@m", maNV.Trim()));
        }
    }
}

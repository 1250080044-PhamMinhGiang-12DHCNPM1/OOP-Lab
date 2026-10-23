using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyNhanXu.Data;
using QuanLyNhanXu.Models;

namespace QuanLyNhanXu.Services
{
    public sealed class KhenThuongKyLuatService
    {
        public void LapQuyetDinhKhenThuong(QuyetDinhTaiChinhInput input)
        {
            ThucHien("dbo.sp_LapQuyetDinhKhenThuong", input);
        }

        public void LapQuyetDinhKyLuat(QuyetDinhTaiChinhInput input)
        {
            ThucHien("dbo.sp_LapQuyetDinhKyLuat", input);
        }

        private static void ThucHien(string procedure, QuyetDinhTaiChinhInput input)
        {
            if (input == null || input.SoTien <= 0 || string.IsNullOrWhiteSpace(input.LyDo)) throw new ArgumentException("Lý do và số tiền là bắt buộc.");
            bool caNhan = input.DoiTuong == "Cá nhân";
            if (caNhan == string.IsNullOrWhiteSpace(input.MaNV)) throw new ArgumentException("Chọn đúng một đối tượng cá nhân hoặc tập thể.");
            Db.Procedure(procedure, null, SqlDbType.Int, 0,
                new SqlParameter("@DoiTuong", caNhan ? "Ca nhan" : "Tap the"),
                new SqlParameter("@MaNV", caNhan ? (object)input.MaNV.Trim() : DBNull.Value),
                new SqlParameter("@MaPhongBan", caNhan ? (object)DBNull.Value : (object)input.MaPhongBan),
                new SqlParameter("@Ngay", input.Ngay.Date), new SqlParameter("@LyDo", input.LyDo.Trim()),
                new SqlParameter("@SoTien", input.SoTien), new SqlParameter("@HinhThuc", input.HinhThuc),
                new SqlParameter("@NguoiKy", input.NguoiKy));
        }

        public DataTable DanhSach()
        {
            return Db.Query(@"SELECT N'Khen thưởng' Loai,MaQDKT MaQD,DoiTuong,ISNULL(MaNV,CONVERT(VARCHAR(10),MaPhongBan)) DoiTuongMa,NgayQuyetDinh,LyDo,SoTien,HinhThucChi HinhThuc FROM QuyetDinhKhenThuong
UNION ALL SELECT N'Kỷ luật',MaQDKL,DoiTuong,ISNULL(MaNV,CONVERT(VARCHAR(10),MaPhongBan)),NgayQuyetDinh,LyDo,SoTien,HinhThucThu FROM QuyetDinhKyLuat ORDER BY NgayQuyetDinh DESC");
        }
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using ChoThueVanPhongCaoOc.Data;

namespace ChoThueVanPhongCaoOc.Services
{
    internal sealed class RentalService
    {
        internal void LapHopDong(string maHd, string maKh, DateTime ngayKy, DateTime ngayBatDau, int soThang, string maPhong, decimal donGia)
        {
            var dsPhong = new DataTable();
            dsPhong.Columns.Add("MaPhong", typeof(string));
            dsPhong.Columns.Add("DonGiaThue", typeof(decimal));
            dsPhong.Rows.Add(maPhong, donGia);
            using (var cn = Db.Open())
            using (var cmd = new SqlCommand("dbo.sp_LapHopDong", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaHD", maHd);
                cmd.Parameters.AddWithValue("@MaKH", maKh);
                cmd.Parameters.AddWithValue("@NgayKy", ngayKy.Date);
                cmd.Parameters.AddWithValue("@NgayBatDau", ngayBatDau.Date);
                cmd.Parameters.AddWithValue("@SoThang", soThang);
                var p = cmd.Parameters.AddWithValue("@PhongThue", dsPhong);
                p.SqlDbType = SqlDbType.Structured;
                p.TypeName = "dbo.DanhSachPhongThue";
                cmd.ExecuteNonQuery();
            }
        }

        internal void ThanhToan(int maKhoanThu, decimal soTien, string ghiChu)
        {
            Exec("dbo.sp_GhiNhanThanhToan", new SqlParameter("@MaKhoanThu", maKhoanThu), new SqlParameter("@SoTien", soTien), new SqlParameter("@GhiChu", (object)ghiChu ?? DBNull.Value));
        }

        internal void LapHoaDonDichVu(int maCthd, string maDv, int thang, int nam, decimal soLuong)
        {
            Exec("dbo.sp_LapHoaDonDichVu", new SqlParameter("@MaCTHD", maCthd), new SqlParameter("@MaDV", maDv), new SqlParameter("@Thang", thang), new SqlParameter("@Nam", nam), new SqlParameter("@SoLuong", soLuong));
        }

        internal void GiaHan(int maCthd, int soThang)
        {
            Exec("dbo.sp_LapPhieuGiaHan", new SqlParameter("@MaCTHD", maCthd), new SqlParameter("@SoThangGiaHan", soThang));
        }

        internal void CapThe(string maThe, string maNv, string bienSo, string loaiXe)
        {
            Exec("dbo.sp_CapTheGiuXe", new SqlParameter("@MaThe", maThe), new SqlParameter("@MaNV", maNv), new SqlParameter("@BienSo", bienSo), new SqlParameter("@LoaiXe", loaiXe));
        }

        private static void Exec(string proc, params SqlParameter[] parameters)
        {
            using (var cn = Db.Open())
            using (var cmd = new SqlCommand(proc, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameters);
                cmd.ExecuteNonQuery();
            }
        }
    }
}

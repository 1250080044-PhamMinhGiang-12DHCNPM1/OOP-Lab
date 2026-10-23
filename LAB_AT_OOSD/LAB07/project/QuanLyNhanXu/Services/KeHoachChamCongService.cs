using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyNhanXu.Data;

namespace QuanLyNhanXu.Services
{
    public sealed class KeHoachChamCongService
    {
        public void LapKeHoachTuan(int maPhongBan, DateTime thuHai, string nguoiLap, string maNV, DateTime ngayLam, int maCa, bool phuTroi)
        {
            Db.Procedure("dbo.sp_LapKeHoachTuan", null, SqlDbType.Int, 0,
                new SqlParameter("@MaPhongBan", maPhongBan), new SqlParameter("@TuNgay", thuHai.Date),
                new SqlParameter("@NguoiLap", nguoiLap), new SqlParameter("@MaNV", maNV.Trim()),
                new SqlParameter("@NgayLam", ngayLam.Date), new SqlParameter("@MaCa", maCa), new SqlParameter("@DuKienPhuTroi", phuTroi));
        }

        public void GhiNhanChamCong(string maNV, DateTime ngay, int maCa, decimal soNgayCong, string trangThai)
        {
            Db.Procedure("dbo.sp_GhiNhanChamCong", null, SqlDbType.Int, 0,
                new SqlParameter("@MaNV", maNV.Trim()), new SqlParameter("@NgayCong", ngay.Date),
                new SqlParameter("@MaCa", maCa), new SqlParameter("@SoNgayCong", soNgayCong), new SqlParameter("@TrangThai", trangThai));
        }

        public void LapPhieuPhuTroi(string maNV, DateTime ngay, string hinhThuc, decimal soGio, string nguoiLap)
        {
            if (soGio <= 0) throw new ArgumentException("Số giờ phụ trội phải lớn hơn 0.");
            Db.Procedure("dbo.sp_LapPhieuPhuTroi", null, SqlDbType.Int, 0,
                new SqlParameter("@MaNV", maNV.Trim()), new SqlParameter("@NgayPhuTroi", ngay.Date),
                new SqlParameter("@HinhThuc", hinhThuc), new SqlParameter("@SoGio", soGio), new SqlParameter("@NguoiLap", nguoiLap));
        }

        public void ChotChamCong(int thang, int nam)
        {
            Db.Procedure("dbo.sp_ChotChamCong", null, SqlDbType.Int, 0, new SqlParameter("@Thang", thang), new SqlParameter("@Nam", nam));
        }

        public DataTable XemChamCong(int thang, int nam)
        {
            return Db.Query("SELECT c.MaNV,n.HoTen,c.NgayCong,c.SoNgayCong,c.TrangThai FROM ChiTietChamCong c JOIN BangChamCong b ON b.MaBangCong=c.MaBangCong JOIN NhanVien n ON n.MaNV=c.MaNV WHERE b.Thang=@t AND b.Nam=@n ORDER BY c.NgayCong,c.MaNV", new SqlParameter("@t", thang), new SqlParameter("@n", nam));
        }
    }
}

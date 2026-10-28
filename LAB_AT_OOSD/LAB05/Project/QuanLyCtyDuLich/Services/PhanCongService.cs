using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyCongTyDuLich.Data;

namespace QuanLyCongTyDuLich.Services
{
    public class PhanCongService
    {
        public DataTable LayDanhSach() { return Db.Query("SELECT p.MaPC,h.HoTen,p.LoaiDoiTuong,ISNULL(p.MaChuyen,p.SoDKDoan) DoiTuong,p.NgayBatDau,p.NgayKetThuc,p.ThuLaoTour FROM PhanCongHDV p JOIN HuongDanVien h ON h.MaHDV=p.MaHDV ORDER BY p.NgayBatDau"); }
        public DataTable LayHDV() { return Db.Query("SELECT MaHDV,MaHDV + N' - ' + HoTen HienThi FROM HuongDanVien WHERE DangLamViec=1"); }
        public KetQuaXuLy PhanCong(string ma,string hdv,string loai,string doiTuong,decimal thuLao)
        {
            try
            {
                DateTime bd,kt; string query = loai=="LE" ? "SELECT NgayDi,NgayVe FROM ChuyenLe WHERE MaChuyen=@ma" : "SELECT NgayDi,NgayKetThucDuKien FROM DangKyDoan WHERE SoDKDoan=@ma AND TrangThai=N'Đã đăng ký'";
                DataTable t=Db.Query(query,new SqlParameter("@ma",doiTuong)); if(t.Rows.Count==0) return KetQuaXuLy.Loi("Đối tượng phân công không hợp lệ."); bd=Convert.ToDateTime(t.Rows[0][0]);kt=Convert.ToDateTime(t.Rows[0][1]);
                object overlap=Db.Scalar("SELECT COUNT(*) FROM PhanCongHDV WHERE MaHDV=@hdv AND NgayBatDau<=@kt AND NgayKetThuc>=@bd",new SqlParameter("@hdv",hdv),new SqlParameter("@bd",bd),new SqlParameter("@kt",kt));
                if(Convert.ToInt32(overlap)>0) return KetQuaXuLy.Loi("Hướng dẫn viên bị chồng chéo lịch.");
                Db.Execute("INSERT PhanCongHDV(MaPC,MaHDV,LoaiDoiTuong,MaChuyen,SoDKDoan,NgayBatDau,NgayKetThuc,ThuLaoTour) VALUES(@pc,@hdv,@loai,@chuyen,@doan,@bd,@kt,@tl)",new SqlParameter("@pc",ma),new SqlParameter("@hdv",hdv),new SqlParameter("@loai",loai),new SqlParameter("@chuyen",loai=="LE"?(object)doiTuong:DBNull.Value),new SqlParameter("@doan",loai=="DOAN"?(object)doiTuong:DBNull.Value),new SqlParameter("@bd",bd),new SqlParameter("@kt",kt),new SqlParameter("@tl",thuLao));
                return KetQuaXuLy.Ok("Đã phân công hướng dẫn viên.");
            }
            catch(Exception ex) { return KetQuaXuLy.Loi(ex.Message); }
        }
    }
}

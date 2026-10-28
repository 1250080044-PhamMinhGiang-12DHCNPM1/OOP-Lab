using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyCongTyDuLich.Data;

namespace QuanLyCongTyDuLich.Services
{
    public class ChuyenLeService
    {
        public DataTable LayChuyen() { return Db.Query("SELECT c.MaChuyen,t.TenTour,c.NgayDi,c.NgayVe,c.DiaDiemDon,c.TrangThai FROM ChuyenLe c JOIN Tour t ON t.MaTour=c.MaTour ORDER BY c.NgayDi"); }
        public DataTable LayChuyenMo() { return Db.Query("SELECT c.MaChuyen,c.MaChuyen + N' - ' + t.TenTour HienThi,t.DonGiaKhach,c.NgayDi,c.NgayVe FROM ChuyenLe c JOIN Tour t ON t.MaTour=c.MaTour WHERE c.TrangThai=N'Mở đăng ký' ORDER BY c.NgayDi"); }
        public KetQuaXuLy ThemChuyen(string ma,string tour,DateTime ngayDi,string diaDiemDon)
        {
            try
            {
                object v = Db.Scalar("SELECT SoNgay FROM Tour WHERE MaTour=@tour AND DangMoBan=1",new SqlParameter("@tour",tour));
                if (v == null) return KetQuaXuLy.Loi("Tour không tồn tại hoặc đã đóng bán.");
                DateTime ngayVe = ngayDi.Date.AddDays(Convert.ToInt32(v)-1);
                Db.Execute("INSERT ChuyenLe VALUES(@ma,@tour,@di,@ve,@don,N'Mở đăng ký')",new SqlParameter("@ma",ma),new SqlParameter("@tour",tour),new SqlParameter("@di",ngayDi.Date),new SqlParameter("@ve",ngayVe),new SqlParameter("@don",diaDiemDon));
                return KetQuaXuLy.Ok("Đã tạo chuyến; ngày về " + ngayVe.ToString("dd/MM/yyyy") + ".");
            }
            catch(Exception ex) { return KetQuaXuLy.Loi(ex.Message); }
        }
        public KetQuaXuLy DongDangKy(string ma) { try { Db.Execute("UPDATE ChuyenLe SET TrangThai=N'Đóng đăng ký' WHERE MaChuyen=@ma",new SqlParameter("@ma",ma)); return KetQuaXuLy.Ok("Đã đóng đăng ký chuyến."); } catch(Exception ex) { return KetQuaXuLy.Loi(ex.Message); } }
    }
}

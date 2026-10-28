using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyCongTyDuLich.Data;

namespace QuanLyCongTyDuLich.Services
{
    public class DanhMucService
    {
        public DataTable LayPhuongTien() { return Db.Query("SELECT MaPT,TenPT,GhiChu FROM PhuongTien ORDER BY MaPT"); }
        public DataTable LayDiemBan() { return Db.Query("SELECT MaDiemBan,TenDiemBan,DiaChi,DienThoai FROM DiemBanVe ORDER BY MaDiemBan"); }
        public DataTable LayHDV() { return Db.Query("SELECT MaHDV,HoTen,DienThoai,LuongCoBan,DangLamViec FROM HuongDanVien ORDER BY MaHDV"); }
        public DataTable LayDiemThamQuan() { return Db.Query("SELECT MaDiemTQ,TenDiemTQ,DiaDiem,NoiDung,YNghia FROM DiemThamQuan ORDER BY MaDiemTQ"); }
        public KetQuaXuLy ThemPhuongTien(string ma, string ten) { return Them("INSERT PhuongTien(MaPT,TenPT) VALUES(@ma,@ten)", "Đã thêm phương tiện.", new SqlParameter("@ma",ma),new SqlParameter("@ten",ten)); }
        public KetQuaXuLy ThemDiemBan(string ma,string ten,string diachi,string dt) { return Them("INSERT DiemBanVe VALUES(@ma,@ten,@dc,@dt)","Đã thêm điểm bán vé.",new SqlParameter("@ma",ma),new SqlParameter("@ten",ten),new SqlParameter("@dc",diachi),new SqlParameter("@dt",dt)); }
        public KetQuaXuLy ThemHDV(string ma,string ten,string dt,decimal luong) { return Them("INSERT HuongDanVien(MaHDV,HoTen,DienThoai,LuongCoBan) VALUES(@ma,@ten,@dt,@luong)","Đã thêm hướng dẫn viên.",new SqlParameter("@ma",ma),new SqlParameter("@ten",ten),new SqlParameter("@dt",dt),new SqlParameter("@luong",luong)); }
        private KetQuaXuLy Them(string sql, string message, params SqlParameter[] p) { try { Db.Execute(sql,p); return KetQuaXuLy.Ok(message); } catch(Exception ex) { return KetQuaXuLy.Loi(ex.Message); } }
    }
}

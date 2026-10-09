using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyCongTyDuLich.Data;

namespace QuanLyCongTyDuLich.Services
{
    public class ThongKeService
    {
        public DataTable LuongHDV(int thang,int nam) { return Db.Query("SELECT h.MaHDV,h.HoTen,h.LuongCoBan,ISNULL(SUM(p.ThuLaoTour),0) ThuLao, h.LuongCoBan+ISNULL(SUM(p.ThuLaoTour),0) LuongThang FROM HuongDanVien h LEFT JOIN PhanCongHDV p ON p.MaHDV=h.MaHDV AND MONTH(p.NgayKetThuc)=@thang AND YEAR(p.NgayKetThuc)=@nam GROUP BY h.MaHDV,h.HoTen,h.LuongCoBan ORDER BY h.MaHDV",new SqlParameter("@thang",thang),new SqlParameter("@nam",nam)); }
        public DataTable TongHop(DateTime tu,DateTime den) { return Db.Query("SELECT (SELECT COUNT(*) FROM DangKyLe WHERE NgayDangKy>=@tu AND NgayDangKy<DATEADD(day,1,@den)) DangKyLe, (SELECT COUNT(*) FROM DangKyDoan WHERE NgayDangKy>=@tu AND NgayDangKy<DATEADD(day,1,@den)) DangKyDoan, (SELECT ISNULL(SUM(ThanhTien),0) FROM DangKyLe WHERE NgayDangKy>=@tu AND NgayDangKy<DATEADD(day,1,@den)) DoanhThuVe, (SELECT ISNULL(SUM(SoTien),0) FROM ThanhToanDoan WHERE NgayThanhToan>=@tu AND NgayThanhToan<DATEADD(day,1,@den)) ThuDoan, (SELECT COUNT(*) FROM KhaoSat WHERE NgayPhanHoi>=@tu AND NgayPhanHoi<DATEADD(day,1,@den)) PhanHoi",new SqlParameter("@tu",tu.Date),new SqlParameter("@den",den.Date)); }
    }
}

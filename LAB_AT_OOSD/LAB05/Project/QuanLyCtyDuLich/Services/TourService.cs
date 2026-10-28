using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyCongTyDuLich.Data;

namespace QuanLyCongTyDuLich.Services
{
    public class TourService
    {
        public DataTable LayTour() { return Db.Query("SELECT MaTour,TenTour,SoNgay,SoDem,DonGiaKhach,MoTa,DangMoBan FROM Tour ORDER BY MaTour"); }
        public DataTable LayTourMoBan() { return Db.Query("SELECT MaTour,TenTour + N' (' + CAST(SoNgay AS nvarchar) + N' ngày)' HienThi,SoNgay,DonGiaKhach FROM Tour WHERE DangMoBan=1 ORDER BY MaTour"); }
        public DataTable LayDiemDung(string maTour) { return Db.Query("SELECT ThuTu,TenDiemDung,DoiPhuongTien,CoNoiAn,CoKhachSan,HangSaoKhachSan FROM TourDiemDung WHERE MaTour=@ma ORDER BY ThuTu",new SqlParameter("@ma",maTour)); }
        public KetQuaXuLy ThemTour(string ma,string ten,int ngay,int dem,decimal gia,string mota)
        {
            if (string.IsNullOrWhiteSpace(ma) || string.IsNullOrWhiteSpace(ten)) return KetQuaXuLy.Loi("Mã và tên tour là bắt buộc.");
            if (ngay <= 0 || dem < 0 || gia < 0) return KetQuaXuLy.Loi("Số ngày, số đêm hoặc đơn giá không hợp lệ.");
            try { Db.Execute("INSERT Tour VALUES(@ma,@ten,@ngay,@dem,@gia,@mota,1)",new SqlParameter("@ma",ma),new SqlParameter("@ten",ten),new SqlParameter("@ngay",ngay),new SqlParameter("@dem",dem),new SqlParameter("@gia",gia),new SqlParameter("@mota",mota)); return KetQuaXuLy.Ok("Đã thêm tour."); }
            catch(Exception ex) { return KetQuaXuLy.Loi(ex.Message); }
        }
        public KetQuaXuLy ThemDiemDung(string maTour,int thuTu,string ten,bool coKhachSan,int? hangSao)
        {
            if (coKhachSan && (!hangSao.HasValue || hangSao < 2 || hangSao > 5)) return KetQuaXuLy.Loi("Khách sạn phải có hạng từ 2 đến 5 sao.");
            try { Db.Execute("INSERT TourDiemDung(MaTour,ThuTu,TenDiemDung,CoKhachSan,HangSaoKhachSan) VALUES(@ma,@tt,@ten,@ks,@sao)",new SqlParameter("@ma",maTour),new SqlParameter("@tt",thuTu),new SqlParameter("@ten",ten),new SqlParameter("@ks",coKhachSan),new SqlParameter("@sao",(object)hangSao ?? DBNull.Value)); return KetQuaXuLy.Ok("Đã thêm điểm dừng."); }
            catch(Exception ex) { return KetQuaXuLy.Loi(ex.Message); }
        }
    }
}

using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyCongTyDuLich.Data;

namespace QuanLyCongTyDuLich.Services
{
    public class KetThucService
    {
        public DataTable DoanCanThanhToan() { return Db.Query("SELECT d.SoDKDoan,dk.TenCoQuanDaiDien,d.NgayKetThucDuKien,d.TongTienDuKien-d.TienCoc-ISNULL(SUM(tt.SoTien),0) ConLai FROM DangKyDoan d JOIN DoanKhach dk ON dk.MaDoan=d.MaDoan LEFT JOIN ThanhToanDoan tt ON tt.SoDKDoan=d.SoDKDoan WHERE d.TrangThai=N'Đã đăng ký' GROUP BY d.SoDKDoan,dk.TenCoQuanDaiDien,d.NgayKetThucDuKien,d.TongTienDuKien,d.TienCoc HAVING d.TongTienDuKien-d.TienCoc-ISNULL(SUM(tt.SoTien),0)>0"); }
        public DataTable LayKhaoSat() { return Db.Query("SELECT MaKhaoSat,LoaiKhach,SoDKLe,SoDKDoan,NgayGui,NgayPhanHoi,DiemDanhGia,GopY FROM KhaoSat ORDER BY NgayGui DESC"); }
        public KetQuaXuLy ThanhToanDoan(string soTT,string soDK,DateTime ngay,decimal tien,string ghiChu)
        {
            try
            {
                DataTable t=Db.Query("SELECT NgayKetThucDuKien,TongTienDuKien-TienCoc-ISNULL((SELECT SUM(SoTien) FROM ThanhToanDoan WHERE SoDKDoan=d.SoDKDoan),0) ConLai FROM DangKyDoan d WHERE SoDKDoan=@so",new SqlParameter("@so",soDK));
                if(t.Rows.Count==0) return KetQuaXuLy.Loi("Không tìm thấy phiếu đoàn.");
                if(ngay.Date<Convert.ToDateTime(t.Rows[0][0]).Date) return KetQuaXuLy.Loi("Kinh phí đoàn chỉ thanh toán sau khi kết thúc chuyến tham quan.");
                decimal con=Convert.ToDecimal(t.Rows[0][1]); if(tien<=0||tien>con) return KetQuaXuLy.Loi("Số tiền vượt số còn phải thanh toán ("+con.ToString("N0")+" đ).");
                Db.Execute("INSERT ThanhToanDoan VALUES(@tt,@dk,@ngay,@tien,@gc)",new SqlParameter("@tt",soTT),new SqlParameter("@dk",soDK),new SqlParameter("@ngay",ngay),new SqlParameter("@tien",tien),new SqlParameter("@gc",ghiChu));
                if(tien==con) Db.Execute("UPDATE DangKyDoan SET TrangThai=N'Đã hoàn tất thanh toán' WHERE SoDKDoan=@so",new SqlParameter("@so",soDK));
                return KetQuaXuLy.Ok("Đã ghi nhận thanh toán đoàn.");
            }
            catch(Exception ex) { return KetQuaXuLy.Loi(ex.Message); }
        }
        public KetQuaXuLy GuiKhaoSat(string ma,string loai,string soDK,DateTime ngayGui)
        {
            try
            {
                string sql = loai=="LE" ? "SELECT NgayVe FROM DangKyLe d JOIN ChuyenLe c ON c.MaChuyen=d.MaChuyen WHERE d.SoDKLe=@so" : "SELECT NgayKetThucDuKien FROM DangKyDoan WHERE SoDKDoan=@so";
                object end=Db.Scalar(sql,new SqlParameter("@so",soDK)); if(end==null||ngayGui.Date<Convert.ToDateTime(end).Date) return KetQuaXuLy.Loi("Phiếu khảo sát chỉ gửi sau khi kết thúc tour.");
                string ins=loai=="LE" ? "INSERT KhaoSat(MaKhaoSat,LoaiKhach,SoDKLe,NgayGui) VALUES(@ma,'LE',@so,@ngay)" : "INSERT KhaoSat(MaKhaoSat,LoaiKhach,SoDKDoan,NgayGui) VALUES(@ma,'DOAN',@so,@ngay)";
                Db.Execute(ins,new SqlParameter("@ma",ma),new SqlParameter("@so",soDK),new SqlParameter("@ngay",ngayGui)); return KetQuaXuLy.Ok("Đã gửi phiếu khảo sát.");
            }
            catch(Exception ex) { return KetQuaXuLy.Loi(ex.Message); }
        }
        public KetQuaXuLy GhiPhanHoi(string ma,DateTime ngay,int diem,string gopY) { if(diem<1||diem>5)return KetQuaXuLy.Loi("Điểm đánh giá từ 1 đến 5."); try{Db.Execute("UPDATE KhaoSat SET NgayPhanHoi=@ngay,DiemDanhGia=@diem,GopY=@gy WHERE MaKhaoSat=@ma",new SqlParameter("@ma",ma),new SqlParameter("@ngay",ngay),new SqlParameter("@diem",diem),new SqlParameter("@gy",gopY));return KetQuaXuLy.Ok("Đã ghi nhận góp ý của khách hàng.");}catch(Exception ex){return KetQuaXuLy.Loi(ex.Message);} }
    }
}

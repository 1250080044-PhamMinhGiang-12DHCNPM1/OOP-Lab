using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyCongTyDuLich.Data;

namespace QuanLyCongTyDuLich.Services
{
    public class DangKyLeService
    {
        public DataTable LayDanhSach() { return Db.Query("SELECT d.SoDKLe,d.TenNguoiDangKy,d.DienThoai,d.SoNguoi,d.ThanhTien,c.MaChuyen,c.NgayDi FROM DangKyLe d JOIN ChuyenLe c ON c.MaChuyen=d.MaChuyen ORDER BY d.NgayDangKy DESC"); }
        public KetQuaXuLy DangKy(string so,string maChuyen,string maDiemBan,string ten,string dt,int soNguoi)
        {
            if (soNguoi < 1 || soNguoi > 11) return KetQuaXuLy.Loi("Khách lẻ chỉ từ 1 đến 11 người (QD01).");
            try
            {
                object gia = Db.Scalar("SELECT t.DonGiaKhach FROM ChuyenLe c JOIN Tour t ON t.MaTour=c.MaTour WHERE c.MaChuyen=@ma AND c.TrangThai=N'Mở đăng ký'",new SqlParameter("@ma",maChuyen));
                if (gia == null) return KetQuaXuLy.Loi("Chuyến đã đóng đăng ký hoặc không tồn tại.");
                decimal thanhTien = Convert.ToDecimal(gia)*soNguoi;
                Db.Execute("INSERT DangKyLe(SoDKLe,MaChuyen,MaDiemBan,TenNguoiDangKy,DienThoai,SoNguoi,ThanhTien) VALUES(@so,@chuyen,@ban,@ten,@dt,@nguoi,@tien)",new SqlParameter("@so",so),new SqlParameter("@chuyen",maChuyen),new SqlParameter("@ban",maDiemBan),new SqlParameter("@ten",ten),new SqlParameter("@dt",dt),new SqlParameter("@nguoi",soNguoi),new SqlParameter("@tien",thanhTien));
                return KetQuaXuLy.Ok("Đã đăng ký và thanh toán vé. Thành tiền: " + thanhTien.ToString("N0") + " đ.");
            }
            catch(Exception ex) { return KetQuaXuLy.Loi(ex.Message); }
        }
    }
}

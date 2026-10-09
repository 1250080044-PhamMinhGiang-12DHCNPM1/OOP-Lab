using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using QuanLyCongTyDuLich.Data;

namespace QuanLyCongTyDuLich.Services
{
    public class DangKyDoanService
    {
        public DataTable LayDanhSach() { return Db.Query("SELECT d.SoDKDoan,dk.TenCoQuanDaiDien,t.TenTour,d.NgayDi,d.NgayKetThucDuKien,d.SoNguoi,d.TienCoc,d.TongTienDuKien,d.TrangThai FROM DangKyDoan d JOIN DoanKhach dk ON dk.MaDoan=d.MaDoan JOIN Tour t ON t.MaTour=d.MaTour ORDER BY d.NgayDi"); }
        public KetQuaXuLy DangKy(string so,string maDoan,string tenCoQuan,string diaChi,string dienThoai,string daiDien,string maTour,DateTime ngayDi,int soNguoi,string diaDiemDon,bool muaBaoHiem,decimal tienCoc,List<string> thanhVien)
        {
            if (soNguoi <= 12) return KetQuaXuLy.Loi("Khách đoàn phải trên 12 người (QD01).");
            if (tienCoc <= 0) return KetQuaXuLy.Loi("Khách đoàn phải đặt cọc trước một khoản tiền.");
            if (muaBaoHiem && (thanhVien == null || thanhVien.Count != soNguoi)) return KetQuaXuLy.Loi("Đoàn mua bảo hiểm phải kèm danh sách đủ " + soNguoi + " người cùng đi.");
            using (var cn = Db.Open())
            using (var tx = cn.BeginTransaction())
            {
                try
                {
                    int soNgay; decimal donGia;
                    using (var get = new SqlCommand("SELECT SoNgay,DonGiaKhach FROM Tour WHERE MaTour=@tour AND DangMoBan=1",cn,tx))
                    { get.Parameters.AddWithValue("@tour",maTour); using (var rd=get.ExecuteReader()) { if(!rd.Read()) return KetQuaXuLy.Loi("Tour không tồn tại hoặc đã đóng bán."); soNgay=rd.GetInt32(0); donGia=rd.GetDecimal(1); } }
                    DateTime ketThuc = ngayDi.Date.AddDays(soNgay-1); decimal tong = donGia*soNguoi;
                    if (tienCoc > tong) return KetQuaXuLy.Loi("Tiền cọc không được vượt tổng dự kiến.");
                    using (var cmd=new SqlCommand("IF EXISTS(SELECT 1 FROM DoanKhach WHERE MaDoan=@ma) UPDATE DoanKhach SET TenCoQuanDaiDien=@ten,DiaChi=@dc,DienThoai=@dt,NguoiDaiDien=@dd WHERE MaDoan=@ma ELSE INSERT DoanKhach VALUES(@ma,@ten,@dc,@dt,@dd)",cn,tx))
                    { cmd.Parameters.AddWithValue("@ma",maDoan);cmd.Parameters.AddWithValue("@ten",tenCoQuan);cmd.Parameters.AddWithValue("@dc",diaChi);cmd.Parameters.AddWithValue("@dt",dienThoai);cmd.Parameters.AddWithValue("@dd",daiDien);cmd.ExecuteNonQuery(); }
                    using (var cmd=new SqlCommand("INSERT DangKyDoan(SoDKDoan,MaDoan,MaTour,NgayDi,NgayKetThucDuKien,SoNguoi,DiaDiemDon,MuaBaoHiem,TienCoc,TongTienDuKien) VALUES(@so,@doan,@tour,@di,@kt,@nguoi,@don,@bh,@coc,@tong)",cn,tx))
                    { cmd.Parameters.AddWithValue("@so",so);cmd.Parameters.AddWithValue("@doan",maDoan);cmd.Parameters.AddWithValue("@tour",maTour);cmd.Parameters.AddWithValue("@di",ngayDi.Date);cmd.Parameters.AddWithValue("@kt",ketThuc);cmd.Parameters.AddWithValue("@nguoi",soNguoi);cmd.Parameters.AddWithValue("@don",diaDiemDon);cmd.Parameters.AddWithValue("@bh",muaBaoHiem);cmd.Parameters.AddWithValue("@coc",tienCoc);cmd.Parameters.AddWithValue("@tong",tong);cmd.ExecuteNonQuery(); }
                    if (muaBaoHiem) for (int i=0;i<thanhVien.Count;i++) using(var cmd=new SqlCommand("INSERT ThanhVienDoan(SoDKDoan,STT,HoTen) VALUES(@so,@stt,@ten)",cn,tx)) { cmd.Parameters.AddWithValue("@so",so);cmd.Parameters.AddWithValue("@stt",i+1);cmd.Parameters.AddWithValue("@ten",thanhVien[i]);cmd.ExecuteNonQuery(); }
                    tx.Commit(); return KetQuaXuLy.Ok("Đã lập phiếu đoàn. Kết thúc dự kiến: " + ketThuc.ToString("dd/MM/yyyy") + ".");
                }
                catch(Exception ex) { tx.Rollback(); return KetQuaXuLy.Loi(ex.Message); }
            }
        }
        public KetQuaXuLy HuyDangKy(string so)
        {
            try { Db.Execute("BEGIN TRANSACTION; DELETE FROM PhanCongHDV WHERE SoDKDoan=@so; UPDATE DangKyDoan SET TrangThai=N'Hủy - mất cọc' WHERE SoDKDoan=@so AND TrangThai=N'Đã đăng ký'; COMMIT",new SqlParameter("@so",so)); return KetQuaXuLy.Ok("Đã hủy phiếu; đoàn mất tiền cọc và phân công liên quan đã được gỡ."); }
            catch(Exception ex) { return KetQuaXuLy.Loi(ex.Message); }
        }
    }
}

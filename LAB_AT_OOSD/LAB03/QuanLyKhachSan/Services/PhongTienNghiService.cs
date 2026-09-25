using System;
using System.Data;
using QuanLyKhachSan.Data;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.Services
{
    public class PhongTienNghiService
    {
        public DataTable LayPhong(string keyword) { return Db.Query(@"SELECT p.SoPhong [Số phòng],p.MaKhuVuc [Mã khu vực],k.TenKhuVuc [Khu vực],p.SoNguoiToiDa [Sức chứa],p.DonGiaNgay [Đơn giá],p.TrangThai [Trạng thái]
FROM Phong p JOIN KhuVuc k ON k.MaKhuVuc=p.MaKhuVuc WHERE p.SoPhong LIKE @k OR k.TenKhuVuc LIKE @k ORDER BY p.SoPhong", Db.P("@k", "%" + (keyword ?? "") + "%")); }
        public DataTable LayTienNghi(string keyword) { return Db.Query(@"SELECT t.MaTienNghi [Mã tiện nghi],t.MaLoaiTN [Mã loại],l.TenLoaiTN [Loại],t.SoThuTu [Số thứ tự],t.TinhTrangHienTai [Tình trạng]
FROM TienNghi t JOIN LoaiTienNghi l ON l.MaLoaiTN=t.MaLoaiTN WHERE t.MaTienNghi LIKE @k OR l.TenLoaiTN LIKE @k ORDER BY t.MaTienNghi", Db.P("@k", "%" + (keyword ?? "") + "%")); }
        public DataTable LayLapDat() { return Db.Query("SELECT SoPhieuLapDat [Số phiếu],MaTienNghi [Mã tiện nghi],SoPhong [Số phòng],NgayLap [Ngày lắp],TinhTrang [Tình trạng],MaNV [Nhân viên] FROM PhieuLapDat ORDER BY NgayLap DESC"); }
        public DataTable LayKhuVuc() { return Db.Query("SELECT MaKhuVuc,TenKhuVuc FROM KhuVuc ORDER BY MaKhuVuc"); }
        public DataTable LayLoaiTienNghi() { return Db.Query("SELECT MaLoaiTN,TenLoaiTN FROM LoaiTienNghi ORDER BY MaLoaiTN"); }
        public KetQua LuuPhong(bool sua, string so, string khu, int sucChua, decimal gia, string trangThai)
        {
            try { if (string.IsNullOrWhiteSpace(so) || string.IsNullOrWhiteSpace(khu) || sucChua <= 0 || gia < 0) return KetQua.Loi("Dữ liệu phòng chưa hợp lệ.");
                if (sua) Db.Execute("UPDATE Phong SET MaKhuVuc=@k,SoNguoiToiDa=@s,DonGiaNgay=@g,TrangThai=@t WHERE SoPhong=@p", Db.P("@k", khu), Db.P("@s", sucChua), Db.P("@g", gia), Db.P("@t", trangThai), Db.P("@p", so));
                else Db.Execute("INSERT Phong(SoPhong,MaKhuVuc,SoNguoiToiDa,DonGiaNgay,TrangThai) VALUES(@p,@k,@s,@g,@t)", Db.P("@p", so), Db.P("@k", khu), Db.P("@s", sucChua), Db.P("@g", gia), Db.P("@t", trangThai)); return KetQua.Ok("Lưu phòng thành công."); }
            catch (Exception ex) { return KetQua.Loi("Không thể lưu phòng. " + ex.Message); }
        }
        public KetQua LuuTienNghi(bool sua, string ma, string loai, int stt, string tinhTrang)
        {
            try { if (string.IsNullOrWhiteSpace(ma) || string.IsNullOrWhiteSpace(loai) || stt <= 0) return KetQua.Loi("Dữ liệu tiện nghi chưa hợp lệ.");
                if (sua) Db.Execute("UPDATE TienNghi SET MaLoaiTN=@l,SoThuTu=@s,TinhTrangHienTai=@t WHERE MaTienNghi=@m", Db.P("@l", loai), Db.P("@s", stt), Db.P("@t", tinhTrang), Db.P("@m", ma));
                else Db.Execute("INSERT TienNghi(MaTienNghi,MaLoaiTN,SoThuTu,TinhTrangHienTai) VALUES(@m,@l,@s,@t)", Db.P("@m", ma), Db.P("@l", loai), Db.P("@s", stt), Db.P("@t", tinhTrang)); return KetQua.Ok("Lưu tiện nghi thành công."); }
            catch (Exception ex) { return KetQua.Loi("Không thể lưu tiện nghi. " + ex.Message); }
        }
        public KetQua Xoa(string table, string key, string value) { try { int n = Db.Execute("DELETE FROM " + table + " WHERE " + key + "=@v", Db.P("@v", value)); return n > 0 ? KetQua.Ok("Xóa thành công.") : KetQua.Loi("Không tìm thấy dữ liệu."); } catch { return KetQua.Loi("Không thể xóa vì dữ liệu đã phát sinh nghiệp vụ."); } }
        public KetQua LapDat(string soPhieu, string maTienNghi, string soPhong, DateTime ngay, string tinhTrang)
        {
            try { if (string.IsNullOrWhiteSpace(soPhieu)) soPhieu = "LD" + DateTime.Now.ToString("yyyyMMddHHmmss"); Db.Execute(@"INSERT PhieuLapDat(SoPhieuLapDat,MaTienNghi,SoPhong,NgayLap,TinhTrang,MaNV) VALUES(@sp,@tn,@p,@n,@tt,@nv)", Db.P("@sp", soPhieu), Db.P("@tn", maTienNghi), Db.P("@p", soPhong), Db.P("@n", ngay.Date), Db.P("@tt", tinhTrang), Db.P("@nv", PhienDangNhap.MaNhanVien)); return KetQua.Ok("Lập phiếu thành công."); }
            catch (Exception ex) { return KetQua.Loi("Thiết bị đã được lắp trong ngày hoặc dữ liệu chưa hợp lệ. " + ex.Message); }
        }
    }
}

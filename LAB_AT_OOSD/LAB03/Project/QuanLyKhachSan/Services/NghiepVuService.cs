using System;
using System.Data;
using QuanLyKhachSan.Data;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.Services
{
    public class NghiepVuService
    {
        public DataTable LayPhongTongQuan() { return Db.Query("SELECT SoPhong [Số phòng],MaKhuVuc [Khu vực],SoNguoiToiDa [Sức chứa],DonGiaNgay [Đơn giá ngày],TrangThai [Trạng thái] FROM Phong ORDER BY SoPhong"); }
        public int DemPhong(string trangThai) { return Convert.ToInt32(Db.Scalar("SELECT COUNT(*) FROM Phong WHERE TrangThai=@t", Db.P("@t", trangThai))); }
        public DataTable LayKhachHang(string keyword) { return Db.Query("SELECT MaKhach [Mã khách],HoTen [Họ tên],SoCMND [CCCD],QuocTich [Quốc tịch],SoDienThoai [Điện thoại] FROM KhachHang WHERE MaKhach LIKE @k OR HoTen LIKE @k OR SoCMND LIKE @k ORDER BY MaKhach", Db.P("@k", "%" + (keyword ?? "") + "%")); }
        public KetQua LuuKhach(bool sua, string ma, string ten, string cccd, string quocTich, string sdt)
        {
            try { if (string.IsNullOrWhiteSpace(ma) || string.IsNullOrWhiteSpace(ten) || string.IsNullOrWhiteSpace(cccd) || string.IsNullOrWhiteSpace(quocTich)) return KetQua.Loi("Vui lòng nhập đủ mã, họ tên, CCCD và quốc tịch.");
                if (sua) Db.Execute("UPDATE KhachHang SET HoTen=@t,SoCMND=@c,QuocTich=@q,SoDienThoai=@s WHERE MaKhach=@m", Db.P("@t", ten), Db.P("@c", cccd), Db.P("@q", quocTich), Db.P("@s", sdt), Db.P("@m", ma));
                else Db.Execute("INSERT KhachHang(MaKhach,HoTen,SoCMND,QuocTich,SoDienThoai) VALUES(@m,@t,@c,@q,@s)", Db.P("@m", ma), Db.P("@t", ten), Db.P("@c", cccd), Db.P("@q", quocTich), Db.P("@s", sdt)); return KetQua.Ok("Lưu khách hàng thành công."); }
            catch (Exception ex) { return KetQua.Loi("Không thể lưu khách hàng. " + ex.Message); }
        }
        public KetQua XoaKhach(string ma) { try { int n = Db.Execute("DELETE KhachHang WHERE MaKhach=@m", Db.P("@m", ma)); return n > 0 ? KetQua.Ok("Xóa khách hàng thành công.") : KetQua.Loi("Không tìm thấy khách hàng."); } catch { return KetQua.Loi("Không thể xóa vì khách hàng đã phát sinh phiếu đặt phòng."); } }
        public DataTable TimPhongTrong(DateTime nhan, DateTime tra, int soKhach)
        {
            if (tra.Date <= nhan.Date) throw new ArgumentException("Ngày trả phải sau ngày nhận.");
            return Db.Query(@"SELECT p.SoPhong [Số phòng],k.TenKhuVuc [Khu vực],p.SoNguoiToiDa [Sức chứa],p.DonGiaNgay [Đơn giá],p.TrangThai [Trạng thái]
FROM Phong p JOIN KhuVuc k ON k.MaKhuVuc=p.MaKhuVuc
WHERE p.SoNguoiToiDa>=@so AND p.TrangThai<>N'Bảo trì' AND NOT EXISTS(
 SELECT 1 FROM ChiTietDatPhong c JOIN PhieuDatPhong d ON d.SoPhieuDat=c.SoPhieuDat
 WHERE c.SoPhong=p.SoPhong AND d.TrangThai IN(N'Đã đặt',N'Đang ở') AND @nhan<d.NgayTraDuKien AND @tra>d.NgayNhan)
ORDER BY p.SoPhong", Db.P("@so", soKhach), Db.P("@nhan", nhan.Date), Db.P("@tra", tra.Date));
        }
        public DataTable LayKhachCombo() { return Db.Query("SELECT MaKhach,MaKhach+' - '+HoTen HienThi FROM KhachHang ORDER BY HoTen"); }
        public KetQua TaoDatPhong(string maKhach, string soPhong, DateTime nhan, DateTime tra, int soKhach, decimal coc, string kenh)
        {
            if (string.IsNullOrWhiteSpace(maKhach) || string.IsNullOrWhiteSpace(soPhong)) return KetQua.Loi("Vui lòng chọn khách hàng và phòng.");
            if (tra.Date <= nhan.Date) return KetQua.Loi("Ngày trả phải sau ngày nhận.");
            string soPhieu = "DP" + DateTime.Now.ToString("yyyyMMddHHmmss");
            try
            {
                Db.Transaction((cn, tx) =>
                {
                    int sucChua = Convert.ToInt32(Db.Scalar(cn, tx, "SELECT SoNguoiToiDa FROM Phong WHERE SoPhong=@p", Db.P("@p", soPhong)));
                    if (soKhach > sucChua) throw new InvalidOperationException("Số khách vượt sức chứa phòng.");
                    int trung = Convert.ToInt32(Db.Scalar(cn, tx, @"SELECT COUNT(*) FROM ChiTietDatPhong c JOIN PhieuDatPhong d ON d.SoPhieuDat=c.SoPhieuDat WHERE c.SoPhong=@p AND d.TrangThai IN(N'Đã đặt',N'Đang ở') AND @nhan<d.NgayTraDuKien AND @tra>d.NgayNhan", Db.P("@p", soPhong), Db.P("@nhan", nhan.Date), Db.P("@tra", tra.Date)));
                    if (trung > 0) throw new InvalidOperationException("Phòng đã được đặt trong khoảng thời gian này.");
                    Db.Execute(cn, tx, @"INSERT PhieuDatPhong(SoPhieuDat,MaKhach,MaNVLeTan,NgayLap,NgayNhan,NgayTraDuKien,TienCoc,KenhDat,TrangThai) VALUES(@sp,@k,@nv,GETDATE(),@nhan,@tra,@coc,@kenh,N'Đã đặt')", Db.P("@sp", soPhieu), Db.P("@k", maKhach), Db.P("@nv", PhienDangNhap.MaNhanVien), Db.P("@nhan", nhan.Date), Db.P("@tra", tra.Date), Db.P("@coc", coc), Db.P("@kenh", kenh));
                    Db.Execute(cn, tx, "INSERT ChiTietDatPhong(SoPhieuDat,SoPhong,SoNguoi) VALUES(@sp,@p,@so)", Db.P("@sp", soPhieu), Db.P("@p", soPhong), Db.P("@so", soKhach));
                    Db.Execute(cn, tx, "UPDATE Phong SET TrangThai=N'Đã đặt' WHERE SoPhong=@p AND TrangThai=N'Trống'", Db.P("@p", soPhong));
                });
                return KetQua.Ok("Lập phiếu " + soPhieu + " thành công.");
            }
            catch (Exception ex) { return KetQua.Loi(ex.Message); }
        }
        public DataTable LayPhieuDat(string trangThai) { return Db.Query(@"SELECT d.SoPhieuDat [Số phiếu],k.HoTen [Khách đặt],d.NgayNhan [Ngày nhận],d.NgayTraDuKien [Ngày trả],c.SoPhong [Phòng],c.SoNguoi [Số khách],d.TrangThai [Trạng thái]
FROM PhieuDatPhong d JOIN KhachHang k ON k.MaKhach=d.MaKhach JOIN ChiTietDatPhong c ON c.SoPhieuDat=d.SoPhieuDat WHERE (@tt='' OR d.TrangThai=@tt) ORDER BY d.NgayLap DESC", Db.P("@tt", trangThai ?? "")); }
        public KetQua ThemNguoiO(string soPhieu, string phong, string ten, string cccd, string quocTich)
        {
            try { int gioiHan = Convert.ToInt32(Db.Scalar("SELECT SoNguoi FROM ChiTietDatPhong WHERE SoPhieuDat=@sp AND SoPhong=@p", Db.P("@sp", soPhieu), Db.P("@p", phong))); int hienTai = Convert.ToInt32(Db.Scalar("SELECT COUNT(*) FROM NguoiLuuTru WHERE SoPhieuDat=@sp AND SoPhong=@p", Db.P("@sp", soPhieu), Db.P("@p", phong))); if (hienTai >= gioiHan) return KetQua.Loi("Đã đủ số người đăng ký."); Db.Execute("INSERT NguoiLuuTru(SoPhieuDat,SoPhong,HoTen,SoCMND,QuocTich) VALUES(@sp,@p,@t,@c,@q)", Db.P("@sp", soPhieu), Db.P("@p", phong), Db.P("@t", ten), Db.P("@c", cccd), Db.P("@q", quocTich)); return KetQua.Ok("Thêm người lưu trú thành công."); } catch (Exception ex) { return KetQua.Loi(ex.Message); }
        }
        public KetQua NhanPhong(string soPhieu, string phong)
        {
            try { Db.Transaction((cn, tx) => { Db.Execute(cn, tx, "UPDATE PhieuDatPhong SET TrangThai=N'Đang ở',NgayNhanThucTe=GETDATE() WHERE SoPhieuDat=@sp AND TrangThai=N'Đã đặt'", Db.P("@sp", soPhieu)); Db.Execute(cn, tx, "UPDATE Phong SET TrangThai=N'Đang ở' WHERE SoPhong=@p", Db.P("@p", phong)); }); return KetQua.Ok("Nhận phòng thành công."); } catch (Exception ex) { return KetQua.Loi(ex.Message); }
        }

        public DataTable LayPhieuDangO() { return Db.Query(@"SELECT d.SoPhieuDat,c.SoPhong,d.SoPhieuDat+' - Phòng '+c.SoPhong HienThi FROM PhieuDatPhong d JOIN ChiTietDatPhong c ON c.SoPhieuDat=d.SoPhieuDat WHERE d.TrangThai=N'Đang ở' ORDER BY d.NgayNhanThucTe DESC"); }
        public DataTable LayDichVuCombo() { return Db.Query("SELECT MaDV,MaDV+' - '+TenDV HienThi,DonGia FROM DichVu ORDER BY TenDV"); }
        public DataTable LayDichVuDaDung(string soPhieu, string phong) { return Db.Query(@"SELECT c.MaDV [Mã dịch vụ],d.TenDV [Tên dịch vụ],c.SoLuong [Số lượng],c.DonGia [Đơn giá],c.ThanhTien [Thành tiền],p.NgaySuDung [Ngày sử dụng] FROM PhieuSuDungDV p JOIN ChiTietPhieuSuDungDV c ON c.SoPhieuSDDV=p.SoPhieuSDDV JOIN DichVu d ON d.MaDV=c.MaDV WHERE p.SoPhieuDat=@sp AND p.SoPhong=@p ORDER BY p.NgaySuDung DESC", Db.P("@sp", soPhieu), Db.P("@p", phong)); }
        public KetQua GhiDichVu(string soPhieu, string phong, DateTime ngay, string maDv, int soLuong)
        {
            if (soLuong <= 0) return KetQua.Loi("Số lượng phải lớn hơn 0.");
            try { Db.Transaction((cn, tx) => { string so = Convert.ToString(Db.Scalar(cn, tx, "SELECT SoPhieuSDDV FROM PhieuSuDungDV WHERE SoPhieuDat=@sp AND SoPhong=@p AND NgaySuDung=@n", Db.P("@sp", soPhieu), Db.P("@p", phong), Db.P("@n", ngay.Date))); if (string.IsNullOrEmpty(so)) { so = "DV" + DateTime.Now.ToString("yyyyMMddHHmmss"); Db.Execute(cn, tx, "INSERT PhieuSuDungDV(SoPhieuSDDV,SoPhieuDat,SoPhong,NgaySuDung,MaNV) VALUES(@so,@sp,@p,@n,@nv)", Db.P("@so", so), Db.P("@sp", soPhieu), Db.P("@p", phong), Db.P("@n", ngay.Date), Db.P("@nv", PhienDangNhap.MaNhanVien)); } decimal gia = Convert.ToDecimal(Db.Scalar(cn, tx, "SELECT DonGia FROM DichVu WHERE MaDV=@m", Db.P("@m", maDv))); int exists = Convert.ToInt32(Db.Scalar(cn, tx, "SELECT COUNT(*) FROM ChiTietPhieuSuDungDV WHERE SoPhieuSDDV=@so AND MaDV=@m", Db.P("@so", so), Db.P("@m", maDv))); if (exists > 0) Db.Execute(cn, tx, "UPDATE ChiTietPhieuSuDungDV SET SoLuong=SoLuong+@sl WHERE SoPhieuSDDV=@so AND MaDV=@m", Db.P("@sl", soLuong), Db.P("@so", so), Db.P("@m", maDv)); else Db.Execute(cn, tx, "INSERT ChiTietPhieuSuDungDV(SoPhieuSDDV,MaDV,SoLuong,DonGia) VALUES(@so,@m,@sl,@g)", Db.P("@so", so), Db.P("@m", maDv), Db.P("@sl", soLuong), Db.P("@g", gia)); }); return KetQua.Ok("Ghi nhận dịch vụ thành công."); } catch (Exception ex) { return KetQua.Loi(ex.Message); }
        }

        public DataTable LayTienNghiPhong(string phong) { return Db.Query(@"SELECT DISTINCT t.MaTienNghi [Mã tiện nghi],l.TenLoaiTN [Tên loại],t.TinhTrangHienTai [Tình trạng trước] FROM PhieuLapDat p JOIN TienNghi t ON t.MaTienNghi=p.MaTienNghi JOIN LoaiTienNghi l ON l.MaLoaiTN=t.MaLoaiTN WHERE p.SoPhong=@p AND p.NgayLap=(SELECT MAX(x.NgayLap) FROM PhieuLapDat x WHERE x.MaTienNghi=p.MaTienNghi) ORDER BY t.MaTienNghi", Db.P("@p", phong)); }
        public KetQua LapDenBu(string soPhieuDat, string phong, string maTienNghi, string mucDo, decimal soTien)
        {
            if (string.IsNullOrWhiteSpace(maTienNghi) || soTien < 0) return KetQua.Loi("Vui lòng chọn tiện nghi và nhập số tiền hợp lệ.");
            string so = "DB" + DateTime.Now.ToString("yyyyMMddHHmmss");
            try { Db.Transaction((cn, tx) => { Db.Execute(cn, tx, "INSERT PhieuDenBu(SoPhieuDenBu,SoPhieuDat,SoPhong,NgayLap,MaNV,TongTien) VALUES(@s,@d,@p,GETDATE(),@nv,@t)", Db.P("@s", so), Db.P("@d", soPhieuDat), Db.P("@p", phong), Db.P("@nv", PhienDangNhap.MaNhanVien), Db.P("@t", soTien)); Db.Execute(cn, tx, "INSERT ChiTietPhieuDenBu(SoPhieuDenBu,MaTienNghi,MucDoThietHai,SoTien) VALUES(@s,@m,@md,@t)", Db.P("@s", so), Db.P("@m", maTienNghi), Db.P("@md", mucDo), Db.P("@t", soTien)); }); return KetQua.Ok("Lập phiếu đền bù " + so + " thành công."); } catch (Exception ex) { return KetQua.Loi(ex.Message); }
        }

        public DataTable LayHoaDon() { return Db.Query(@"SELECT h.SoHoaDon [Số hóa đơn],h.SoPhieuDat [Phiếu đặt],h.SoNgayTinhTien [Số đêm],h.TienPhong [Tiền phòng],h.TienDichVu [Tiền dịch vụ],h.TongTien [Tổng tiền],ISNULL((SELECT SUM(SoTien) FROM ThanhToan t WHERE t.SoHoaDon=h.SoHoaDon),0) [Đã thanh toán],h.TrangThai [Trạng thái] FROM HoaDon h ORDER BY h.NgayLap DESC"); }
        public KetQua LapHoaDon(string soPhieu)
        {
            try { string soHd = "HD" + DateTime.Now.ToString("yyyyMMddHHmmss"); Db.Transaction((cn, tx) => { int days = Convert.ToInt32(Db.Scalar(cn, tx, "SELECT CASE WHEN DATEDIFF(day,NgayNhan,CAST(GETDATE() AS date))<1 THEN 1 ELSE DATEDIFF(day,NgayNhan,CAST(GETDATE() AS date)) END FROM PhieuDatPhong WHERE SoPhieuDat=@sp", Db.P("@sp", soPhieu))); decimal room = Convert.ToDecimal(Db.Scalar(cn, tx, "SELECT SUM(p.DonGiaNgay)*@d FROM ChiTietDatPhong c JOIN Phong p ON p.SoPhong=c.SoPhong WHERE c.SoPhieuDat=@sp", Db.P("@d", days), Db.P("@sp", soPhieu))); decimal service = Convert.ToDecimal(Db.Scalar(cn, tx, "SELECT ISNULL(SUM(c.ThanhTien),0) FROM PhieuSuDungDV p JOIN ChiTietPhieuSuDungDV c ON c.SoPhieuSDDV=p.SoPhieuSDDV WHERE p.SoPhieuDat=@sp", Db.P("@sp", soPhieu))); Db.Execute(cn, tx, "INSERT HoaDon(SoHoaDon,SoPhieuDat,NgayLap,MaNV,SoNgayTinhTien,TienPhong,TienDichVu) VALUES(@hd,@sp,GETDATE(),@nv,@d,@p,@dv)", Db.P("@hd", soHd), Db.P("@sp", soPhieu), Db.P("@nv", PhienDangNhap.MaNhanVien), Db.P("@d", days), Db.P("@p", room), Db.P("@dv", service)); }); return KetQua.Ok("Lập hóa đơn " + soHd + " thành công."); } catch (Exception ex) { return KetQua.Loi("Không thể lập hóa đơn. " + ex.Message); }
        }
        public KetQua ThanhToan(string soHoaDon, string hinhThuc, decimal soTien)
        {
            try { decimal tong = Convert.ToDecimal(Db.Scalar("SELECT TongTien FROM HoaDon WHERE SoHoaDon=@h", Db.P("@h", soHoaDon))); decimal da = Convert.ToDecimal(Db.Scalar("SELECT ISNULL(SUM(SoTien),0) FROM ThanhToan WHERE SoHoaDon=@h", Db.P("@h", soHoaDon))); if (soTien <= 0 || soTien > tong - da) return KetQua.Loi("Số tiền phải lớn hơn 0 và không vượt số tiền còn thiếu."); string ma = "TT" + DateTime.Now.ToString("yyyyMMddHHmmss"); Db.Transaction((cn, tx) => { Db.Execute(cn, tx, "INSERT ThanhToan(MaThanhToan,SoHoaDon,NgayThanhToan,HinhThuc,SoTien) VALUES(@m,@h,GETDATE(),@ht,@t)", Db.P("@m", ma), Db.P("@h", soHoaDon), Db.P("@ht", hinhThuc), Db.P("@t", soTien)); if (da + soTien == tong) Db.Execute(cn, tx, "UPDATE HoaDon SET TrangThai=N'Đã thanh toán' WHERE SoHoaDon=@h", Db.P("@h", soHoaDon)); }); return KetQua.Ok("Ghi nhận thanh toán thành công."); } catch (Exception ex) { return KetQua.Loi(ex.Message); }
        }
        public KetQua TraPhong(string soHoaDon)
        {
            try { string state = Convert.ToString(Db.Scalar("SELECT TrangThai FROM HoaDon WHERE SoHoaDon=@h", Db.P("@h", soHoaDon))); if (state != "Đã thanh toán") return KetQua.Loi("Hóa đơn chưa được thanh toán đủ."); Db.Transaction((cn, tx) => { string booking = Convert.ToString(Db.Scalar(cn, tx, "SELECT SoPhieuDat FROM HoaDon WHERE SoHoaDon=@h", Db.P("@h", soHoaDon))); Db.Execute(cn, tx, "UPDATE PhieuDatPhong SET TrangThai=N'Đã trả',NgayTraThucTe=GETDATE() WHERE SoPhieuDat=@sp", Db.P("@sp", booking)); Db.Execute(cn, tx, "UPDATE Phong SET TrangThai=N'Trống' WHERE SoPhong IN(SELECT SoPhong FROM ChiTietDatPhong WHERE SoPhieuDat=@sp)", Db.P("@sp", booking)); }); return KetQua.Ok("Trả phòng thành công."); } catch (Exception ex) { return KetQua.Loi(ex.Message); }
        }
        public DataTable TongHop(DateTime tu, DateTime den) { return Db.Query(@"SELECT CAST(h.NgayLap AS date) [Ngày],COUNT(*) [Số hóa đơn],SUM(h.TongTien) [Doanh thu],ISNULL((SELECT SUM(d.TongTien) FROM PhieuDenBu d WHERE CAST(d.NgayLap AS date)=CAST(h.NgayLap AS date)),0) [Đền bù] FROM HoaDon h WHERE CAST(h.NgayLap AS date) BETWEEN @tu AND @den GROUP BY CAST(h.NgayLap AS date) ORDER BY [Ngày]", Db.P("@tu", tu.Date), Db.P("@den", den.Date)); }
        public DataTable ThongKeDichVu(DateTime tu, DateTime den) { return Db.Query(@"SELECT d.MaDV [Mã dịch vụ],d.TenDV [Tên dịch vụ],SUM(c.SoLuong) [Tổng số lượng],SUM(c.ThanhTien) [Tổng tiền] FROM PhieuSuDungDV p JOIN ChiTietPhieuSuDungDV c ON c.SoPhieuSDDV=p.SoPhieuSDDV JOIN DichVu d ON d.MaDV=c.MaDV WHERE p.NgaySuDung BETWEEN @tu AND @den GROUP BY d.MaDV,d.TenDV ORDER BY [Tổng tiền] DESC", Db.P("@tu", tu.Date), Db.P("@den", den.Date)); }
        public DataTable ChiSo(DateTime tu, DateTime den) { return Db.Query(@"SELECT (SELECT COUNT(*) FROM PhieuDatPhong WHERE CAST(NgayLap AS date) BETWEEN @tu AND @den) SoPhieuDat,(SELECT COUNT(*) FROM PhieuDatPhong WHERE TrangThai=N'Đang ở') DangO,(SELECT ISNULL(SUM(TongTien),0) FROM HoaDon WHERE CAST(NgayLap AS date) BETWEEN @tu AND @den) DoanhThu,(SELECT ISNULL(SUM(TongTien),0) FROM PhieuDenBu WHERE CAST(NgayLap AS date) BETWEEN @tu AND @den) DenBu", Db.P("@tu", tu.Date), Db.P("@den", den.Date)); }
    }
}

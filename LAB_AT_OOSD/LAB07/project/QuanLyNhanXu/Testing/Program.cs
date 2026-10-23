using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyNhanXu.Data;
using QuanLyNhanXu.Models;
using QuanLyNhanXu.Services;

namespace QuanLyNhanSu.Tests
{
    internal static class Program
    {
        private static int _passed, _failed;
        private static string _maNV;
        private static readonly TuyenDungService TuyenDung = new TuyenDungService();
        private static readonly HopDongBaoHiemService HopDong = new HopDongBaoHiemService();
        private static readonly QuyetDinhService QuyetDinh = new QuyetDinhService();
        private static readonly KeHoachChamCongService ChamCong = new KeHoachChamCongService();
        private static readonly KhenThuongKyLuatService ThuongPhat = new KhenThuongKyLuatService();
        private static readonly BangLuongService Luong = new BangLuongService();

        private static void Main()
        {
            try
            {
                CleanupByCccd();
                Run("TC01 - Tiếp nhận thử việc hợp lệ", TiepNhanThuViec);
                Run("TC02 - Chặn ký hợp đồng trước khi thử việc đạt", ChanKySom);
                Run("TC03 - Hợp đồng dài hạn và bảo hiểm", HopDongDaiHanBaoHiem);
                Run("TC04 - Quyết định lương và chức vụ theo hiệu lực", QuyetDinhLuongChucVu);
                Run("TC05 - Kế hoạch tuần và 24 ngày công", KeHoachVaChamCong);
                Run("TC06 - Phụ trội đúng 10 giờ hệ số 1,5", PhuTroi);
                Run("TC07 - Khen thưởng và kỷ luật liên kết phiếu", KhenThuongKyLuat);
                Run("TC08 - Chặn tính lương khi bảng công chưa chốt", ChanTinhLuongSom);
                Run("TC09 - Tính lương chuẩn 13.902.308 đồng", TinhLuongChuan);
                Run("TC10 - Chặn tính lương trùng và chốt bảng lương", ChanTrungVaChot);
            }
            finally { Cleanup(); }
            Console.WriteLine("SUMMARY | Passed={0} | Failed={1}", _passed, _failed);
            Environment.ExitCode = _failed == 0 ? 0 : 1;
        }

        private static void TiepNhanThuViec()
        {
            _maNV = TuyenDung.TiepNhanThuViec(new ThuViecInput { HoTen = "Nguyễn Văn An Test", NgaySinh = new DateTime(2000, 1, 1), GioiTinh = "Nam", CCCD = "TEST1250080044", DienThoai = "0900000044", DiaChi = "TP.HCM", MaPhongBan = 1, MaNhuCau = 1, TuNgay = new DateTime(2026, 1, 1), DenNgayThuViec = new DateTime(2026, 1, 31), LuongThuViec = 8000000m, DieuKhoan = "Thử việc hai bên theo thỏa thuận" });
            Assert(!string.IsNullOrWhiteSpace(_maNV), "Không sinh mã nhân viên.");
            Assert(Scalar("SELECT COUNT(*) FROM QuyetDinhTuyenDung WHERE MaNV=@m", new SqlParameter("@m", _maNV)) == 1, "Thiếu quyết định tuyển dụng.");
        }

        private static void ChanKySom()
        {
            Assert(Throws(delegate { HopDong.KyHopDongChinhThuc(new HopDongInput { MaNV = _maNV, LoaiHopDong = "Dai han", NgayKy = new DateTime(2026, 1, 15), TuNgay = new DateTime(2026, 1, 20) }); }), "Hệ thống cho ký hợp đồng khi chưa đạt thử việc.");
        }

        private static void HopDongDaiHanBaoHiem()
        {
            TuyenDung.DanhGiaThuViec(_maNV, true);
            int id = HopDong.KyHopDongChinhThuc(new HopDongInput { MaNV = _maNV, LoaiHopDong = "Dai han", NgayKy = new DateTime(2026, 2, 1), TuNgay = new DateTime(2026, 2, 1) });
            Assert(id > 0, "Không tạo hợp đồng."); HopDong.TaoHoSoBaoHiem(_maNV, "BHXH-TEST-0044", "BHYT-TEST-0044", new DateTime(2026, 2, 1));
            Assert(Scalar("SELECT COUNT(*) FROM BaoHiem WHERE MaNV=@m", new SqlParameter("@m", _maNV)) == 1, "Thiếu hồ sơ bảo hiểm.");
        }

        private static void QuyetDinhLuongChucVu()
        {
            QuyetDinh.BanHanhQuyetDinhLuong(_maNV, new DateTime(2026, 2, 1), 12000000m, 500000m, 300000m);
            QuyetDinh.BanHanhQuyetDinhChucVu(_maNV, 2, new DateTime(2026, 2, 1), 1500000m);
            Assert(Scalar("SELECT COUNT(*) FROM QuyetDinhLuong WHERE MaNV=@m", new SqlParameter("@m", _maNV)) == 1, "Thiếu quyết định lương.");
        }

        private static void KeHoachVaChamCong()
        {
            ChamCong.LapKeHoachTuan(1, new DateTime(2026, 9, 7), "Trưởng phòng", _maNV, new DateTime(2026, 9, 7), 1, true);
            for (int i = 1; i <= 24; i++) ChamCong.GhiNhanChamCong(_maNV, new DateTime(2026, 9, i), 1, 1m, "Co mat");
            Assert(Scalar("SELECT COUNT(*) FROM ChiTietChamCong c JOIN BangChamCong b ON b.MaBangCong=c.MaBangCong WHERE c.MaNV=@m AND b.Thang=9 AND b.Nam=2026", new SqlParameter("@m", _maNV)) == 24, "Không đủ 24 ngày công.");
        }

        private static void PhuTroi()
        {
            ChamCong.LapPhieuPhuTroi(_maNV, new DateTime(2026, 9, 15), "Tang ca", 10m, "Ban chấm công");
            decimal factor = ScalarDecimal("SELECT p.HeSo FROM PhieuPhuTroi p JOIN ChiTietPhuTroi c ON c.MaPhieuPT=p.MaPhieuPT WHERE c.MaNV=@m", new SqlParameter("@m", _maNV));
            Assert(factor == 1.5m, "Hệ số phụ trội không bằng 1,5.");
        }

        private static void KhenThuongKyLuat()
        {
            ThuongPhat.LapQuyetDinhKhenThuong(new QuyetDinhTaiChinhInput { DoiTuong = "Cá nhân", MaNV = _maNV, Ngay = new DateTime(2026, 9, 20), LyDo = "Hoàn thành tốt nhiệm vụ", SoTien = 1000000m, HinhThuc = "Cong luong", NguoiKy = "Giám đốc" });
            ThuongPhat.LapQuyetDinhKyLuat(new QuyetDinhTaiChinhInput { DoiTuong = "Cá nhân", MaNV = _maNV, Ngay = new DateTime(2026, 9, 21), LyDo = "Vi phạm nội quy", SoTien = 200000m, HinhThuc = "Tru luong", NguoiKy = "Giám đốc" });
            Assert(Scalar("SELECT COUNT(*) FROM PhieuChiKhenThuong p JOIN QuyetDinhKhenThuong q ON q.MaQDKT=p.MaQDKT WHERE q.MaNV=@m", new SqlParameter("@m", _maNV)) == 1, "Thiếu phiếu chi.");
            Assert(Scalar("SELECT COUNT(*) FROM PhieuThuNopPhat p JOIN QuyetDinhKyLuat q ON q.MaQDKL=p.MaQDKL WHERE q.MaNV=@m", new SqlParameter("@m", _maNV)) == 1, "Thiếu phiếu thu.");
        }

        private static void ChanTinhLuongSom() { Assert(Throws(delegate { Luong.TinhLuongThang(9, 2026); }), "Cho tính lương trước khi chốt bảng công."); }
        private static void TinhLuongChuan()
        {
            ChamCong.ChotChamCong(9, 2026); Luong.TinhLuongThang(9, 2026);
            decimal actual = ScalarDecimal("SELECT c.ThucLinh FROM ChiTietBangLuong c JOIN BangLuong b ON b.MaBangLuong=c.MaBangLuong WHERE c.MaNV=@m AND b.Thang=9 AND b.Nam=2026", new SqlParameter("@m", _maNV));
            Assert(actual == 13902308m, "Thực lĩnh sai: " + actual);
        }
        private static void ChanTrungVaChot() { Assert(Throws(delegate { Luong.TinhLuongThang(9, 2026); }), "Cho tính lương trùng."); Luong.ChotBangLuong(9, 2026); Assert(Scalar("SELECT COUNT(*) FROM BangLuong WHERE Thang=9 AND Nam=2026 AND TrangThai=N'Da chot'") == 1, "Bảng lương chưa chốt."); }

        private static void Run(string name, Action test) { try { test(); _passed++; Console.WriteLine("PASS | " + name); } catch (Exception ex) { _failed++; Console.WriteLine("FAIL | {0} | {1}", name, ex.Message); } }
        private static bool Throws(Action action) { try { action(); return false; } catch { return true; } }
        private static void Assert(bool value, string message) { if (!value) throw new InvalidOperationException(message); }
        private static int Scalar(string sql, params SqlParameter[] p) { using (var c = Db.OpenConnection()) using (var cmd = new SqlCommand(sql, c)) { if (p != null) cmd.Parameters.AddRange(p); return Convert.ToInt32(cmd.ExecuteScalar()); } }
        private static decimal ScalarDecimal(string sql, params SqlParameter[] p) { using (var c = Db.OpenConnection()) using (var cmd = new SqlCommand(sql, c)) { if (p != null) cmd.Parameters.AddRange(p); return Convert.ToDecimal(cmd.ExecuteScalar()); } }
        private static void CleanupByCccd() { _maNV = Convert.ToString(Db.Query("SELECT MaNV FROM NhanVien WHERE CCCD='TEST1250080044'").Rows.Count > 0 ? Db.Query("SELECT MaNV FROM NhanVien WHERE CCCD='TEST1250080044'").Rows[0][0] : null); Cleanup(); }
        private static void Cleanup()
        {
            if (string.IsNullOrWhiteSpace(_maNV)) return;
            const string sql = @"DELETE c FROM ChiTietBangLuong c WHERE c.MaNV=@m; DELETE FROM BangLuong WHERE NOT EXISTS(SELECT 1 FROM ChiTietBangLuong c WHERE c.MaBangLuong=BangLuong.MaBangLuong);
DELETE p FROM PhieuChiKhenThuong p JOIN QuyetDinhKhenThuong q ON q.MaQDKT=p.MaQDKT WHERE q.MaNV=@m; DELETE FROM QuyetDinhKhenThuong WHERE MaNV=@m;
DELETE p FROM PhieuThuNopPhat p JOIN QuyetDinhKyLuat q ON q.MaQDKL=p.MaQDKL WHERE q.MaNV=@m; DELETE FROM QuyetDinhKyLuat WHERE MaNV=@m;
DELETE c FROM ChiTietPhuTroi c WHERE c.MaNV=@m; DELETE FROM PhieuPhuTroi WHERE NOT EXISTS(SELECT 1 FROM ChiTietPhuTroi c WHERE c.MaPhieuPT=PhieuPhuTroi.MaPhieuPT);
DELETE FROM ChiTietChamCong WHERE MaNV=@m; DELETE FROM BangChamCong WHERE NOT EXISTS(SELECT 1 FROM ChiTietChamCong c WHERE c.MaBangCong=BangChamCong.MaBangCong);
DELETE FROM ChiTietKeHoach WHERE MaNV=@m; DELETE FROM KeHoachTuan WHERE NOT EXISTS(SELECT 1 FROM ChiTietKeHoach c WHERE c.MaKeHoach=KeHoachTuan.MaKeHoach);
DELETE FROM QuyetDinhChucVu WHERE MaNV=@m; DELETE FROM QuyetDinhLuong WHERE MaNV=@m; DELETE FROM BaoHiem WHERE MaNV=@m; DELETE FROM HopDongLaoDong WHERE MaNV=@m; DELETE FROM QuyetDinhTuyenDung WHERE MaNV=@m; DELETE FROM NhanVien WHERE MaNV=@m;";
            try { Db.Execute(sql, new SqlParameter("@m", _maNV)); } catch { } _maNV = null;
        }
    }
}

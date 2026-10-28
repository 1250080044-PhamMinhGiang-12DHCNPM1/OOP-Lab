using System;
using System.Collections.Generic;
using System.Data;
using QuanLyCongTyDuLich.Data;
using QuanLyCongTyDuLich.Services;

namespace Task4ServiceTests
{
    internal static class Program
    {
        private static int passed;
        private static int failed;

        private static void Check(string id, string title, bool condition, string detail)
        {
            if (condition)
            {
                passed++;
                Console.WriteLine("PASS | " + id + " | " + title + " | " + detail);
            }
            else
            {
                failed++;
                Console.WriteLine("FAIL | " + id + " | " + title + " | " + detail);
            }
        }

        private static void Clean()
        {
            Db.Execute("DELETE FROM KhaoSat WHERE MaKhaoSat='KST01'; DELETE FROM ThanhToanDoan WHERE SoTT='TTT01'; DELETE FROM PhanCongHDV WHERE MaPC='PCT01'; DELETE FROM DangKyLe WHERE SoDKLe='TDKL01'; DELETE FROM ThanhVienDoan WHERE SoDKDoan='TDD01'; DELETE FROM DangKyDoan WHERE SoDKDoan='TDD01'; DELETE FROM DoanKhach WHERE MaDoan='TDOAN01'; DELETE FROM ChuyenLe WHERE MaChuyen='TCL01';");
        }

        private static bool HasRow(string sql, string expected)
        {
            object value = Db.Scalar(sql);
            return value != null && Convert.ToString(value) == expected;
        }

        public static int Main()
        {
            try
            {
                Clean();
                var chuyen = new ChuyenLeService();
                var le = new DangKyLeService();
                var doan = new DangKyDoanService();
                var pc = new PhanCongService();
                var ketThuc = new KetThucService();
                var thongKe = new ThongKeService();

                var r = chuyen.ThemChuyen("TCL01", "T001", new DateTime(2026, 11, 5), "Điểm đón kiểm thử");
                Check("TC01", "Tạo chuyến tính ngày về", r.ThanhCong && HasRow("SELECT CONVERT(varchar(10),NgayVe,23) FROM ChuyenLe WHERE MaChuyen='TCL01'", "2026-11-07"), r.ThongBao);

                r = le.DangKy("TDKL01", "CL002", "DB01", "Khách kiểm thử", "0909000001", 12);
                Check("TC02", "Từ chối khách lẻ 12 người", !r.ThanhCong, r.ThongBao);

                r = le.DangKy("TDKL01", "CL002", "DB01", "Khách kiểm thử", "0909000001", 2);
                Check("TC03", "Đăng ký và thanh toán khách lẻ", r.ThanhCong && HasRow("SELECT CONVERT(varchar(30),ThanhTien) FROM DangKyLe WHERE SoDKLe='TDKL01'", "5000000.00"), r.ThongBao);

                r = doan.DangKy("TDD01", "TDOAN01", "Công ty kiểm thử", "1 Đường Test", "0283999999", "Người kiểm thử", "T001", new DateTime(2026, 11, 20), 12, "Điểm đón test", false, 1000000, null);
                Check("TC04", "Từ chối đoàn không quá 12 người", !r.ThanhCong, r.ThongBao);

                r = doan.DangKy("TDD01", "TDOAN01", "Công ty kiểm thử", "1 Đường Test", "0283999999", "Người kiểm thử", "T001", new DateTime(2026, 11, 20), 13, "Điểm đón test", false, 0, null);
                Check("TC05", "Từ chối đoàn không đặt cọc", !r.ThanhCong, r.ThongBao);

                r = doan.DangKy("TDD01", "TDOAN01", "Công ty kiểm thử", "1 Đường Test", "0283999999", "Người kiểm thử", "T001", new DateTime(2026, 11, 20), 13, "Điểm đón test", true, 5000000, new List<string> { "01", "02" });
                Check("TC06", "Từ chối bảo hiểm thiếu danh sách", !r.ThanhCong, r.ThongBao);

                var members = new List<string>();
                for (int i = 1; i <= 13; i++) members.Add("Thành viên kiểm thử " + i);
                r = doan.DangKy("TDD01", "TDOAN01", "Công ty kiểm thử", "1 Đường Test", "0283999999", "Người kiểm thử", "T001", new DateTime(2026, 11, 20), 13, "Điểm đón test", true, 5000000, members);
                Check("TC07", "Lập phiếu đoàn có bảo hiểm", r.ThanhCong && HasRow("SELECT CONVERT(varchar(10),NgayKetThucDuKien,23) FROM DangKyDoan WHERE SoDKDoan='TDD01'", "2026-11-22") && HasRow("SELECT COUNT(*) FROM ThanhVienDoan WHERE SoDKDoan='TDD01'", "13"), r.ThongBao);

                r = pc.PhanCong("PCT01", "HDV01", "LE", "CL001", 1200000);
                Check("TC08", "Từ chối HDV chồng lịch", !r.ThanhCong, r.ThongBao);

                r = pc.PhanCong("PCT01", "HDV01", "DOAN", "TDD01", 1200000);
                Check("TC09", "Phân công HDV đoàn", r.ThanhCong, r.ThongBao);

                r = ketThuc.ThanhToanDoan("TTT01", "TDD01", new DateTime(2026, 11, 21), 1000000, "Thử trước ngày kết thúc");
                Check("TC10", "Từ chối thanh toán trước kết thúc", !r.ThanhCong, r.ThongBao);

                r = ketThuc.ThanhToanDoan("TTT01", "TDD01", new DateTime(2026, 11, 23), 1000000, "Thanh toán sau tour");
                Check("TC11", "Ghi nhận thanh toán sau kết thúc", r.ThanhCong && HasRow("SELECT COUNT(*) FROM ThanhToanDoan WHERE SoTT='TTT01'", "1"), r.ThongBao);

                r = ketThuc.GuiKhaoSat("KST01", "DOAN", "TDD01", new DateTime(2026, 11, 21));
                Check("TC12", "Từ chối khảo sát trước kết thúc", !r.ThanhCong, r.ThongBao);

                r = ketThuc.GuiKhaoSat("KST01", "DOAN", "TDD01", new DateTime(2026, 11, 23));
                Check("TC13", "Gửi khảo sát sau kết thúc", r.ThanhCong, r.ThongBao);

                r = ketThuc.GhiPhanHoi("KST01", new DateTime(2026, 11, 24), 6, "Ngoài thang điểm");
                Check("TC14", "Từ chối điểm khảo sát ngoài thang", !r.ThanhCong, r.ThongBao);

                r = ketThuc.GhiPhanHoi("KST01", new DateTime(2026, 11, 24), 5, "Dịch vụ tốt");
                Check("TC15", "Ghi nhận phản hồi hợp lệ", r.ThanhCong && HasRow("SELECT CONVERT(varchar(10),DiemDanhGia) FROM KhaoSat WHERE MaKhaoSat='KST01'", "5"), r.ThongBao);

                DataTable salary = thongKe.LuongHDV(11, 2026);
                DataRow[] h = salary.Select("MaHDV='HDV01'");
                Check("TC16", "Tính lương theo tháng", h.Length == 1 && Convert.ToDecimal(h[0]["LuongThang"]) == 10200000m, "Lương HDV01 tháng 11/2026 = " + (h.Length == 1 ? Convert.ToDecimal(h[0]["LuongThang"]).ToString("N0") : "không tìm thấy") + " đ.");

                DataTable summary = thongKe.TongHop(new DateTime(2026, 9, 1), new DateTime(2026, 9, 30));
                Check("TC17", "Thống kê theo khoảng thời gian", summary.Rows.Count == 1 && Convert.ToInt32(summary.Rows[0]["DangKyDoan"]) == 1 && Convert.ToInt32(summary.Rows[0]["PhanHoi"]) == 1, "Tháng 09/2026: 1 đăng ký đoàn, 1 phản hồi.");
            }
            catch (Exception ex)
            {
                failed++;
                Console.WriteLine("FAIL | SYSTEM | Lỗi không mong đợi | " + ex.Message);
            }
            finally
            {
                try { Clean(); } catch (Exception ex) { failed++; Console.WriteLine("FAIL | CLEANUP | " + ex.Message); }
            }

            Console.WriteLine("SUMMARY | Passed=" + passed + " | Failed=" + failed);
            return failed == 0 ? 0 : 1;
        }
    }
}

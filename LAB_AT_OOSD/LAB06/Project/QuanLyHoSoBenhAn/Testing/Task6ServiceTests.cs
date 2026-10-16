using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Task6ServiceTests
{
    internal static class Program
    {
        private static int _passed;
        private static int _failed;
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["QuanLyHoSoBenhAnDb"].ConnectionString;
        private static string _phone;
        private static string _record;
        private static int _visitId;

        private static void Main()
        {
            _phone = "098" + DateTime.Now.ToString("HHmmssfff");
            try
            {
                Run("TC01 - Đăng ký lần đầu, thu lệ phí và cấp hồ sơ", RegisterAndCreateRecord);
                Run("TC02 - Chặn bệnh nhân đã có hồ sơ", DuplicateRecordIsRejected);
                Run("TC03 - Tra cứu lịch sử theo mã hồ sơ", SearchHistory);
                Run("TC04 - Đặt lịch định kỳ và chặn trùng lịch", AppointmentRules);
                Run("TC05 - Phiếu khám liên kết xét nghiệm và toa", CreateClinicalRecord);
                Run("TC06 - Thanh toán thì hoàn tất điều trị", PayAndComplete);
                Run("TC07 - Chặn thanh toán lặp", DuplicatePaymentIsRejected);
            }
            finally
            {
                Cleanup();
            }
            Console.WriteLine("SUMMARY | Passed={0} | Failed={1}", _passed, _failed);
            Environment.ExitCode = _failed == 0 ? 0 : 1;
        }

        private static void Run(string name, Action test)
        {
            try { test(); _passed++; Console.WriteLine("PASS | " + name); }
            catch (Exception ex) { _failed++; Console.WriteLine("FAIL | {0} | {1}", name, ex.Message); }
        }

        private static SqlConnection Open() { var c = new SqlConnection(ConnectionString); c.Open(); return c; }
        private static void Assert(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
        private static int ScalarInt(string sql, params SqlParameter[] parameters)
        {
            using (var c = Open()) using (var cmd = new SqlCommand(sql, c)) { cmd.Parameters.AddRange(parameters); return Convert.ToInt32(cmd.ExecuteScalar()); }
        }
        private static bool Throws(Action action) { try { action(); return false; } catch (SqlException) { return true; } }

        private static void RegisterAndCreateRecord()
        {
            using (var c = Open()) using (var cmd = new SqlCommand("dbo.sp_DangKyCapHoSo", c))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HoTen", "Bệnh nhân test"); cmd.Parameters.AddWithValue("@NgaySinh", new DateTime(2000, 1, 1));
                cmd.Parameters.AddWithValue("@DiaChi", "TP.HCM"); cmd.Parameters.AddWithValue("@DienThoai", _phone); cmd.Parameters.AddWithValue("@Anh3x4", DBNull.Value);
                cmd.Parameters.AddWithValue("@SoTienLePhi", 200000m); cmd.Parameters.AddWithValue("@Nam", 2026);
                var output = new SqlParameter("@MaHoSo", SqlDbType.VarChar, 20) { Direction = ParameterDirection.Output }; cmd.Parameters.Add(output);
                cmd.ExecuteNonQuery(); _record = Convert.ToString(output.Value);
            }
            Assert(!string.IsNullOrWhiteSpace(_record), "Không sinh mã hồ sơ.");
            Assert(ScalarInt("SELECT COUNT(*) FROM LePhiNam l JOIN HoSoBenhAn h ON l.MaBN=h.MaBN WHERE h.MaHoSo=@h", new SqlParameter("@h", _record)) == 1, "Thiếu lệ phí hoặc hồ sơ.");
        }

        private static void DuplicateRecordIsRejected()
        {
            Assert(Throws(() =>
            {
                using (var c = Open()) using (var cmd = new SqlCommand("dbo.sp_DangKyCapHoSo", c))
                {
                    cmd.CommandType = CommandType.StoredProcedure; cmd.Parameters.AddWithValue("@HoTen", "Trùng"); cmd.Parameters.AddWithValue("@NgaySinh", new DateTime(2000, 1, 1));
                    cmd.Parameters.AddWithValue("@DiaChi", "TP.HCM"); cmd.Parameters.AddWithValue("@DienThoai", _phone); cmd.Parameters.AddWithValue("@Anh3x4", DBNull.Value); cmd.Parameters.AddWithValue("@SoTienLePhi", 200000m); cmd.Parameters.AddWithValue("@Nam", 2026);
                    cmd.Parameters.Add(new SqlParameter("@MaHoSo", SqlDbType.VarChar, 20) { Direction = ParameterDirection.Output }); cmd.ExecuteNonQuery();
                }
            }), "Hệ thống cho phép cấp hồ sơ trùng.");
        }

        private static void SearchHistory() { Assert(ScalarInt("SELECT COUNT(*) FROM HoSoBenhAn WHERE MaHoSo=@h", new SqlParameter("@h", _record)) == 1, "Không tra được hồ sơ vừa cấp."); }
        private static void AppointmentRules()
        {
            var time = DateTime.Today.AddDays(14).AddHours(9);
            using (var c = Open()) using (var cmd = new SqlCommand("INSERT LichHen(MaHoSo,NgayGioHen,LoaiHen) VALUES(@h,@t,N'Định kỳ')", c)) { cmd.Parameters.AddWithValue("@h", _record); cmd.Parameters.AddWithValue("@t", time); cmd.ExecuteNonQuery(); }
            Assert(Throws(() => { using (var c = Open()) using (var cmd = new SqlCommand("INSERT LichHen(MaHoSo,NgayGioHen,LoaiHen) VALUES(@h,@t,N'Định kỳ')", c)) { cmd.Parameters.AddWithValue("@h", _record); cmd.Parameters.AddWithValue("@t", time); cmd.ExecuteNonQuery(); } }), "Hệ thống cho phép trùng lịch hẹn.");
        }

        private static void CreateClinicalRecord()
        {
            var details = new DataTable(); details.Columns.Add("MaThuoc", typeof(int)); details.Columns.Add("SoLuong", typeof(int)); details.Columns.Add("LieuDung", typeof(string)); details.Columns.Add("CachDung", typeof(string)); details.Rows.Add(1, 10, "1 viên/lần", "Sau ăn");
            using (var c = Open()) using (var cmd = new SqlCommand("dbo.sp_LapPhieuKhamVaToa", c))
            {
                cmd.CommandType = CommandType.StoredProcedure; cmd.Parameters.AddWithValue("@MaHoSo", _record); cmd.Parameters.AddWithValue("@MaBS", 1); cmd.Parameters.AddWithValue("@MaKhoa", 1);
                cmd.Parameters.AddWithValue("@TrieuChung", "Đau đầu"); cmd.Parameters.AddWithValue("@ChanDoan", "Cảm cúm"); cmd.Parameters.AddWithValue("@NoiDungXetNghiem", "Công thức máu"); cmd.Parameters.AddWithValue("@LoiDan", "Uống đủ nước");
                var p = cmd.Parameters.AddWithValue("@ChiTiet", details); p.SqlDbType = SqlDbType.Structured; p.TypeName = "dbo.TT_ChiTietToa";
                var visit = new SqlParameter("@MaPK", SqlDbType.Int) { Direction = ParameterDirection.Output }; cmd.Parameters.Add(visit); cmd.Parameters.Add(new SqlParameter("@MaToa", SqlDbType.Int) { Direction = ParameterDirection.Output }); cmd.ExecuteNonQuery(); _visitId = Convert.ToInt32(visit.Value);
            }
            Assert(ScalarInt("SELECT COUNT(*) FROM ToaThuoc t JOIN ChiTietToaThuoc ct ON t.MaToa=ct.MaToa WHERE t.MaPK=@p", new SqlParameter("@p", _visitId)) == 1, "Toa thuốc/chi tiết toa không liên kết với phiếu khám.");
            Assert(ScalarInt("SELECT COUNT(*) FROM ChiDinhXetNghiem WHERE MaPK=@p", new SqlParameter("@p", _visitId)) == 1, "Thiếu chỉ định xét nghiệm.");
        }
        private static void PayAndComplete()
        {
            using (var c = Open()) using (var cmd = new SqlCommand("dbo.sp_ThanhToanHoanTat", c))
            {
                cmd.CommandType = CommandType.StoredProcedure; cmd.Parameters.AddWithValue("@MaPK", _visitId); cmd.Parameters.AddWithValue("@SoTien", 150000m); cmd.Parameters.AddWithValue("@PhuongThuc", "Tiền mặt"); cmd.Parameters.Add(new SqlParameter("@MaTT", SqlDbType.Int) { Direction = ParameterDirection.Output }); cmd.ExecuteNonQuery();
            }
            Assert(ScalarInt("SELECT COUNT(*) FROM PhieuKham WHERE MaPK=@p AND TrangThai=N'Hoàn tất'", new SqlParameter("@p", _visitId)) == 1, "Phiếu khám chưa hoàn tất sau thanh toán.");
        }
        private static void DuplicatePaymentIsRejected()
        {
            Assert(Throws(() => { using (var c = Open()) using (var cmd = new SqlCommand("dbo.sp_ThanhToanHoanTat", c)) { cmd.CommandType = CommandType.StoredProcedure; cmd.Parameters.AddWithValue("@MaPK", _visitId); cmd.Parameters.AddWithValue("@SoTien", 150000m); cmd.Parameters.AddWithValue("@PhuongThuc", "Tiền mặt"); cmd.Parameters.Add(new SqlParameter("@MaTT", SqlDbType.Int) { Direction = ParameterDirection.Output }); cmd.ExecuteNonQuery(); } }), "Hệ thống cho phép thanh toán lặp.");
        }
        private static void Cleanup()
        {
            if (string.IsNullOrWhiteSpace(_phone)) return;
            const string sql = @"DECLARE @bn INT=(SELECT MaBN FROM BenhNhan WHERE DienThoai=@phone); DECLARE @h VARCHAR(20)=(SELECT MaHoSo FROM HoSoBenhAn WHERE MaBN=@bn);
DELETE FROM ThanhToanVienPhi WHERE MaPK IN (SELECT MaPK FROM PhieuKham WHERE MaHoSo=@h); DELETE FROM ChiTietToaThuoc WHERE MaToa IN (SELECT MaToa FROM ToaThuoc WHERE MaPK IN (SELECT MaPK FROM PhieuKham WHERE MaHoSo=@h)); DELETE FROM ToaThuoc WHERE MaPK IN (SELECT MaPK FROM PhieuKham WHERE MaHoSo=@h); DELETE FROM KetQuaXetNghiem WHERE MaCDXN IN (SELECT MaCDXN FROM ChiDinhXetNghiem WHERE MaPK IN (SELECT MaPK FROM PhieuKham WHERE MaHoSo=@h)); DELETE FROM ChiDinhXetNghiem WHERE MaPK IN (SELECT MaPK FROM PhieuKham WHERE MaHoSo=@h); DELETE FROM LichHen WHERE MaHoSo=@h; DELETE FROM PhieuKham WHERE MaHoSo=@h; DELETE FROM HoSoBenhAn WHERE MaBN=@bn; DELETE FROM LePhiNam WHERE MaBN=@bn; DELETE FROM PhieuDangKy WHERE MaBN=@bn; DELETE FROM BenhNhan WHERE MaBN=@bn;";
            try { using (var c = Open()) using (var cmd = new SqlCommand(sql, c)) { cmd.Parameters.AddWithValue("@phone", _phone); cmd.ExecuteNonQuery(); } } catch { }
        }
    }
}

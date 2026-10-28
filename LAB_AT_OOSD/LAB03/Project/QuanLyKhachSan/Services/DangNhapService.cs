using System;
using System.Data;
using System.Security.Cryptography;
using QuanLyKhachSan.Data;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.Services
{
    public class DangNhapService
    {
        public KetQua DangNhap(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password)) return KetQua.Loi("Vui lòng nhập tên đăng nhập và mật khẩu.");
            try
            {
                var table = Db.Query(@"SELECT t.TenDangNhap,t.MaNV,t.MatKhauHash,t.MatKhauSalt,t.SoVongLap,n.HoTen,n.VaiTro
FROM TaiKhoan t JOIN NhanVien n ON n.MaNV=t.MaNV WHERE t.TenDangNhap=@u AND t.DangHoatDong=1", Db.P("@u", username.Trim()));
                if (table.Rows.Count == 0) return KetQua.Loi("Tài khoản không tồn tại hoặc đã bị khóa.");
                DataRow row = table.Rows[0]; byte[] expected = (byte[])row["MatKhauHash"]; byte[] salt = (byte[])row["MatKhauSalt"]; int rounds = Convert.ToInt32(row["SoVongLap"]);
                byte[] actual; using (var derive = new Rfc2898DeriveBytes(password, salt, rounds, HashAlgorithmName.SHA256)) actual = derive.GetBytes(32);
                if (!FixedEquals(expected, actual)) return KetQua.Loi("Tên đăng nhập hoặc mật khẩu chưa đúng.");
                PhienDangNhap.TenDangNhap = Convert.ToString(row["TenDangNhap"]); PhienDangNhap.MaNhanVien = Convert.ToString(row["MaNV"]); PhienDangNhap.HoTen = Convert.ToString(row["HoTen"]); PhienDangNhap.VaiTro = Convert.ToString(row["VaiTro"]);
                Db.Execute("UPDATE TaiKhoan SET LanDangNhapCuoi=GETDATE() WHERE TenDangNhap=@u", Db.P("@u", username.Trim()));
                return KetQua.Ok("Đăng nhập thành công.");
            }
            catch (Exception ex) { return KetQua.Loi("Không kết nối được database. " + ex.Message); }
        }
        private static bool FixedEquals(byte[] a, byte[] b) { if (a == null || b == null || a.Length != b.Length) return false; int diff = 0; for (int i = 0; i < a.Length; i++) diff |= a[i] ^ b[i]; return diff == 0; }
    }
}

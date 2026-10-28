namespace QuanLyKhachSan.Models
{
    public static class PhienDangNhap
    {
        public static string TenDangNhap { get; set; }
        public static string MaNhanVien { get; set; }
        public static string HoTen { get; set; }
        public static string VaiTro { get; set; }
        public static void Xoa() { TenDangNhap = MaNhanVien = HoTen = VaiTro = null; }
    }
}

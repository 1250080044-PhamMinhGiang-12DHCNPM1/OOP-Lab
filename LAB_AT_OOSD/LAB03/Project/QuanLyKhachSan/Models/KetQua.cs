namespace QuanLyKhachSan.Models
{
    public class KetQua
    {
        public bool ThanhCong { get; private set; }
        public string ThongBao { get; private set; }
        private KetQua(bool ok, string message) { ThanhCong = ok; ThongBao = message; }
        public static KetQua Ok(string message) { return new KetQua(true, message); }
        public static KetQua Loi(string message) { return new KetQua(false, message); }
    }
}

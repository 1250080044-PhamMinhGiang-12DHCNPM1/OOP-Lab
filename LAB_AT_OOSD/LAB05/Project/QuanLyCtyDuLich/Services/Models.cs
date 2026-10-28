namespace QuanLyCongTyDuLich.Services
{
    public class KetQuaXuLy
    {
        public bool ThanhCong { get; private set; }
        public string ThongBao { get; private set; }
        public static KetQuaXuLy Ok(string message) { return new KetQuaXuLy { ThanhCong = true, ThongBao = message }; }
        public static KetQuaXuLy Loi(string message) { return new KetQuaXuLy { ThanhCong = false, ThongBao = message }; }
    }
}

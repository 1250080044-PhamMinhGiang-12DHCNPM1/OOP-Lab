namespace WindowsFormsApp1.Models
{
    public class MucGioHang
    {
        public SanPham SanPham { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien { get { return SanPham.DonGia * SoLuong; } }
    }
}

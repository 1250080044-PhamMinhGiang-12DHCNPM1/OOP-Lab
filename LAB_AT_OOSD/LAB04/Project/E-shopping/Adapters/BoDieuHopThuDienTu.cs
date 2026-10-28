namespace WindowsFormsApp1.Adapters
{
    // Mô phỏng lời gọi đến dịch vụ thư điện tử.
    public class BoDieuHopThuDienTu
    {
        public bool GuiXacNhan(string thuDienTu, int maDonHang)
        {
            return !string.IsNullOrWhiteSpace(thuDienTu) && thuDienTu.Contains("@");
        }
    }
}

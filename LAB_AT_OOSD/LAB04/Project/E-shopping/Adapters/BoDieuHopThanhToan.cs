using System.Text.RegularExpressions;

namespace WindowsFormsApp1.Adapters
{
    // Mô phỏng lời gọi đến dịch vụ thanh toán trực tuyến.
    public class BoDieuHopThanhToan
    {
        public bool ThanhToan(string soThe, decimal soTien)
        {
            return soTien > 0 && !string.IsNullOrWhiteSpace(soThe) && Regex.IsMatch(soThe, "^[0-9]{6,}$");
        }
    }
}

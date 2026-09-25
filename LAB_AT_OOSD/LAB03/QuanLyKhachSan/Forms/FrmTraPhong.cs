using System;
using System.Drawing.Printing;
using System.Windows.Forms;
namespace QuanLyKhachSan.Forms
{
    public partial class FrmTraPhong : Form
    {
        private readonly PrintDocument hoaDon = new PrintDocument();
        public FrmTraPhong() { InitializeComponent(); hoaDon.PrintPage += (s, e) => e.Graphics.DrawString("HÓA ĐƠN KHÁCH SẠN\n\nSố hóa đơn: HD0001\nTổng tiền: 0 VNĐ", new System.Drawing.Font("Segoe UI", 14), System.Drawing.Brushes.Black, 80, 80); }
        private void btnInHoaDon_Click(object sender, EventArgs e) { using (var preview = new PrintPreviewDialog { Document = hoaDon, Width = 900, Height = 650 }) preview.ShowDialog(this); }
    }
}

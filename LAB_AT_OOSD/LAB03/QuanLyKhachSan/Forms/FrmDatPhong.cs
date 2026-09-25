using System;
using System.Windows.Forms;
namespace QuanLyKhachSan.Forms
{
    public partial class FrmDatPhong : Form
    {
        public FrmDatPhong() { InitializeComponent(); }
        private void btnTimPhong_Click(object sender, EventArgs e) { lblKetQua.Text = "Tìm thấy 3 phòng phù hợp"; }
        private void btnLapPhieu_Click(object sender, EventArgs e) { MessageBox.Show("Giao diện đã sẵn sàng. Nghiệp vụ lưu database sẽ được nối ở bước 3.", "Lập phiếu đặt phòng", MessageBoxButtons.OK, MessageBoxIcon.Information); }
    }
}

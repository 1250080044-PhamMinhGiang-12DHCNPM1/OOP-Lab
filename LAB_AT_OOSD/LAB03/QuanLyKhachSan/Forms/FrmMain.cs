using System;
using System.Windows.Forms;

namespace QuanLyKhachSan.Forms
{
    public partial class FrmMain : Form
    {
        public FrmMain() { InitializeComponent(); }
        private void Open(Form form) { using (form) form.ShowDialog(this); }
        private void btnDanhMuc_Click(object s, EventArgs e) { Open(new FrmDanhMuc()); }
        private void btnPhong_Click(object s, EventArgs e) { Open(new FrmPhongTienNghi()); }
        private void btnDatPhong_Click(object s, EventArgs e) { Open(new FrmDatPhong()); }
        private void btnDichVu_Click(object s, EventArgs e) { Open(new FrmDichVu()); }
        private void btnTraPhong_Click(object s, EventArgs e) { Open(new FrmTraPhong()); }
        private void btnThongKe_Click(object s, EventArgs e) { Open(new FrmThongKe()); }
        private void btnDangXuat_Click(object s, EventArgs e) { Close(); }
    }
}

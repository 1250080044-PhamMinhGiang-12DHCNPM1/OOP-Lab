using System;
using System.Windows.Forms;
using QuanLyKhachSan.Services;

namespace QuanLyKhachSan.Forms
{
    public partial class FrmDangNhap : Form
    {
        private readonly DangNhapService service = new DangNhapService();
        public FrmDangNhap() { InitializeComponent(); }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            var result = service.DangNhap(txtTenDangNhap.Text, txtMatKhau.Text);
            if (result.ThanhCong)
            {
                Hide();
                using (var main = new FrmMain()) main.ShowDialog();
                Show(); txtMatKhau.Clear(); txtMatKhau.Focus();
            }
            else MessageBox.Show(result.ThongBao, "Đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e) { txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked; }
        private void btnThoat_Click(object sender, EventArgs e) { Close(); }
    }
}

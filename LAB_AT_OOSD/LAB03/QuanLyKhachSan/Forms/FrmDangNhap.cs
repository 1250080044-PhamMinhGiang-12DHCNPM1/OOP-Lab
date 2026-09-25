using System;
using System.Windows.Forms;

namespace QuanLyKhachSan.Forms
{
    public partial class FrmDangNhap : Form
    {
        public FrmDangNhap() { InitializeComponent(); }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (txtTenDangNhap.Text.Trim() == "admin" && txtMatKhau.Text == "123")
            {
                Hide();
                using (var main = new FrmMain()) main.ShowDialog();
                Show(); txtMatKhau.Clear(); txtMatKhau.Focus();
            }
            else MessageBox.Show("Tên đăng nhập hoặc mật khẩu chưa đúng.\nTài khoản giao diện mẫu: admin / 123", "Đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e) { txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked; }
        private void btnThoat_Click(object sender, EventArgs e) { Close(); }
    }
}

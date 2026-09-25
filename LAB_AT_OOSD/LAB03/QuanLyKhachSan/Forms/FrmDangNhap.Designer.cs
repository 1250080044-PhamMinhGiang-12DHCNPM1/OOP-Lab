using System.Drawing;
using System.Windows.Forms;

namespace QuanLyKhachSan.Forms
{
    partial class FrmDangNhap
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtTenDangNhap; private TextBox txtMatKhau; private CheckBox chkHienMatKhau; private Button btnDangNhap; private Button btnThoat;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container(); UiTheme.StyleForm(this, "Đăng nhập - Quản lý khách sạn");
            ClientSize = new Size(980, 620); FormBorderStyle = FormBorderStyle.FixedSingle; MaximizeBox = false;
            var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 }; root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 47)); root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 53));
            var brand = new Panel { Dock = DockStyle.Fill, BackColor = UiTheme.Navy };
            var brandTitle = new Label { Text = "QUẢN LÝ\nKHÁCH SẠN", ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 28F), AutoSize = true, Location = new Point(52, 145) };
            var brandText = new Label { Text = "Quản lý phòng, đặt phòng, dịch vụ\nvà thanh toán trong một hệ thống.", ForeColor = Color.FromArgb(205, 220, 242), Font = new Font("Segoe UI", 12F), AutoSize = true, Location = new Point(56, 255) };
            brand.Controls.Add(brandTitle); brand.Controls.Add(brandText);
            var right = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            var title = UiTheme.Heading("Đăng nhập hệ thống", 23F); title.Location = new Point(70, 105);
            var subtitle = new Label { Text = "Sử dụng tài khoản nhân viên được cấp", AutoSize = true, ForeColor = Color.DimGray, Location = new Point(74, 153) };
            var lblUser = new Label { Text = "Tên đăng nhập", AutoSize = true, Font = new Font("Segoe UI Semibold", 10F), Location = new Point(74, 205) };
            txtTenDangNhap = new TextBox { Location = new Point(74, 232), Size = new Size(360, 30), Text = "admin" };
            var lblPass = new Label { Text = "Mật khẩu", AutoSize = true, Font = new Font("Segoe UI Semibold", 10F), Location = new Point(74, 285) };
            txtMatKhau = new TextBox { Location = new Point(74, 312), Size = new Size(360, 30), Text = "123", UseSystemPasswordChar = true };
            chkHienMatKhau = new CheckBox { Text = "Hiện mật khẩu", AutoSize = true, Location = new Point(74, 355) }; chkHienMatKhau.CheckedChanged += chkHienMatKhau_CheckedChanged;
            btnDangNhap = UiTheme.Button("Đăng nhập", true); btnDangNhap.Location = new Point(74, 405); btnDangNhap.Width = 230; btnDangNhap.Click += btnDangNhap_Click;
            btnThoat = UiTheme.Button("Thoát", false); btnThoat.Location = new Point(314, 405); btnThoat.Width = 120; btnThoat.Click += btnThoat_Click;
            var note = new Label { Text = "Tài khoản mẫu: admin / 123", AutoSize = true, ForeColor = UiTheme.Blue, Location = new Point(74, 470) };
            right.Controls.AddRange(new Control[] { title, subtitle, lblUser, txtTenDangNhap, lblPass, txtMatKhau, chkHienMatKhau, btnDangNhap, btnThoat, note });
            root.Controls.Add(brand, 0, 0); root.Controls.Add(right, 1, 0); Controls.Add(root); AcceptButton = btnDangNhap; CancelButton = btnThoat;
        }
    }
}

using System.Drawing;
using System.Windows.Forms;
namespace QuanLyKhachSan.Forms
{
    partial class FrmDanhMuc
    {
        private System.ComponentModel.IContainer components = null; private TabControl tabs;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container(); UiTheme.StyleForm(this, "Quản lý danh mục"); ClientSize = new Size(1180, 730);
            var header = new Panel { Dock = DockStyle.Top, Height = 82, BackColor = Color.White, Padding = new Padding(24, 18, 24, 10) };
            var title = UiTheme.Heading("Quản lý danh mục", 22F); var note = new Label { Text = "Dữ liệu nền dùng chung cho toàn hệ thống", AutoSize = true, ForeColor = Color.DimGray, Location = new Point(27, 52) }; header.Controls.Add(title); header.Controls.Add(note);
            tabs = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10F), Padding = new Point(16, 7) };
            tabs.TabPages.Add(UiTheme.CrudPage("Khu vực", new[] { "Mã khu vực", "Tên khu vực" }, new[] { "Mã khu vực", "Tên khu vực" }));
            tabs.TabPages.Add(UiTheme.CrudPage("Nhân viên", new[] { "Mã NV", "Họ tên", "Vai trò", "Số điện thoại" }, new[] { "Mã nhân viên", "Họ tên", "Vai trò", "Số điện thoại" }));
            tabs.TabPages.Add(UiTheme.CrudPage("Loại tiện nghi", new[] { "Mã loại", "Tên loại" }, new[] { "Mã loại", "Tên loại" }));
            tabs.TabPages.Add(UiTheme.CrudPage("Dịch vụ", new[] { "Mã DV", "Tên dịch vụ", "Đơn vị", "Đơn giá" }, new[] { "Mã dịch vụ", "Tên dịch vụ", "Đơn vị tính", "Đơn giá" }));
            tabs.TabPages.Add(UiTheme.CrudPage("Quy định đền bù", new[] { "Mã QĐ", "Loại tiện nghi", "Mức độ", "Mức đền bù" }, new[] { "Mã quy định", "Loại tiện nghi", "Mức độ thiệt hại", "Mức đền bù" }));
            Controls.Add(tabs); Controls.Add(header);
        }
    }
}

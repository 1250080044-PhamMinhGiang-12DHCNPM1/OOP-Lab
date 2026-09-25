using System.Drawing;
using System.Windows.Forms;

namespace QuanLyKhachSan.Forms
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnDanhMuc, btnPhong, btnDatPhong, btnDichVu, btnTraPhong, btnThongKe, btnDangXuat;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private Button MenuButton(string text)
        {
            var b = new Button { Text = text, Height = 52, Dock = DockStyle.Top, FlatStyle = FlatStyle.Flat, BackColor = UiTheme.Navy, ForeColor = Color.White, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(24, 0, 0, 0), Cursor = Cursors.Hand };
            b.FlatAppearance.BorderSize = 0; b.FlatAppearance.MouseOverBackColor = UiTheme.Blue; return b;
        }
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container(); UiTheme.StyleForm(this, "Tổng quan - Quản lý khách sạn"); ClientSize = new Size(1220, 760); WindowState = FormWindowState.Maximized;
            var side = new Panel { Dock = DockStyle.Left, Width = 235, BackColor = UiTheme.Navy };
            var logo = new Label { Text = "HOTEL MANAGER", Dock = DockStyle.Top, Height = 92, ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 18F), TextAlign = ContentAlignment.MiddleCenter };
            btnDangXuat = MenuButton("   Đăng xuất"); btnDangXuat.Dock = DockStyle.Bottom; btnDangXuat.Click += btnDangXuat_Click;
            btnThongKe = MenuButton("   Thống kê"); btnThongKe.Click += btnThongKe_Click;
            btnTraPhong = MenuButton("   Trả phòng - Thanh toán"); btnTraPhong.Click += btnTraPhong_Click;
            btnDichVu = MenuButton("   Sử dụng dịch vụ"); btnDichVu.Click += btnDichVu_Click;
            btnDatPhong = MenuButton("   Đặt - Nhận phòng"); btnDatPhong.Click += btnDatPhong_Click;
            btnPhong = MenuButton("   Phòng - Tiện nghi"); btnPhong.Click += btnPhong_Click;
            btnDanhMuc = MenuButton("   Danh mục"); btnDanhMuc.Click += btnDanhMuc_Click;
            side.Controls.AddRange(new Control[] { btnThongKe, btnTraPhong, btnDichVu, btnDatPhong, btnPhong, btnDanhMuc, logo, btnDangXuat });
            var content = new Panel { Dock = DockStyle.Fill, Padding = new Padding(32), BackColor = UiTheme.Background };
            var header = new TableLayoutPanel { Dock = DockStyle.Top, Height = 78, ColumnCount = 2 }; header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70)); header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            var hTitle = UiTheme.Heading("Tổng quan khách sạn", 24F); hTitle.Margin = new Padding(0, 8, 0, 0);
            var user = new Label { Text = "Phạm Mai Anh\nQuản lý", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight, ForeColor = UiTheme.Text };
            header.Controls.Add(hTitle, 0, 0); header.Controls.Add(user, 1, 0);
            var cards = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 132, Padding = new Padding(0, 8, 0, 8) };
            cards.Controls.Add(UiTheme.Card("Phòng trống", "03", Color.FromArgb(35, 165, 92))); cards.Controls.Add(UiTheme.Card("Đã đặt", "00", Color.FromArgb(245, 158, 11)));
            cards.Controls.Add(UiTheme.Card("Đang ở", "00", UiTheme.Blue)); cards.Controls.Add(UiTheme.Card("Bảo trì", "00", Color.FromArgb(220, 68, 68)));
            var section = UiTheme.Heading("Tình trạng phòng hôm nay", 17F); section.Dock = DockStyle.Top; section.Height = 48;
            var grid = UiTheme.Grid("Số phòng", "Khu vực", "Sức chứa", "Đơn giá ngày", "Trạng thái");
            grid.Rows.Add("A101", "Khu A", "2", "600.000", "Trống"); grid.Rows.Add("A102", "Khu A", "3", "800.000", "Trống"); grid.Rows.Add("B201", "Khu B", "4", "1.200.000", "Trống");
            content.Controls.Add(grid); content.Controls.Add(section); content.Controls.Add(cards); content.Controls.Add(header); Controls.Add(content); Controls.Add(side);
        }
    }
}

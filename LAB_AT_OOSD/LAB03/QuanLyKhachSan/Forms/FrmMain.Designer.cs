using System.Drawing;
using System.Windows.Forms;

namespace QuanLyKhachSan.Forms
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnDanhMuc;
        private Button btnPhong;
        private Button btnDatPhong;
        private Button btnDichVu;
        private Button btnTraPhong;
        private Button btnThongKe;
        private Button btnDangXuat;
        private Label lblNguoiDung;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private Button TaoNutChucNang(string text)
        {
            var button = new Button
            {
                Text = text,
                Dock = DockStyle.Fill,
                Margin = new Padding(9),
                BackColor = Color.FromArgb(248, 249, 251),
                ForeColor = Color.FromArgb(35, 45, 58),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(28, 0, 8, 0),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            button.FlatAppearance.BorderColor = Color.FromArgb(155, 164, 175);
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(231, 241, 252);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(211, 230, 249);
            return button;
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            Text = "Quản lý khách sạn";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(900, 530);
            MinimumSize = new Size(916, 569);
            MaximumSize = new Size(916, 569);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(245, 247, 250);
            Font = new Font("Segoe UI", 10F);

            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 112,
                BackColor = Color.White
            };
            pnlHeader.Paint += delegate(object sender, PaintEventArgs e)
            {
                using (var pen = new Pen(Color.FromArgb(205, 217, 231)))
                    e.Graphics.DrawLine(pen, 0, pnlHeader.Height - 1,
                        pnlHeader.Width, pnlHeader.Height - 1);
            };

            var lblTieuDe = new Label
            {
                Text = "HỆ THỐNG QUẢN LÝ KHÁCH SẠN",
                Dock = DockStyle.Fill,
                ForeColor = Color.FromArgb(24, 73, 125),
                Font = new Font("Segoe UI Semibold", 22F),
                TextAlign = ContentAlignment.MiddleCenter
            };
            lblNguoiDung = new Label
            {
                Name = "lblNguoiDung",
                Text = "Người dùng - Vai trò",
                AutoSize = false,
                Size = new Size(270, 28),
                Location = new Point(610, 76),
                ForeColor = Color.DimGray,
                Font = new Font("Segoe UI", 9F),
                TextAlign = ContentAlignment.MiddleRight,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            pnlHeader.Controls.Add(lblTieuDe);
            pnlHeader.Controls.Add(lblNguoiDung);

            var tableMenu = new TableLayoutPanel
            {
                Location = new Point(25, 135),
                Size = new Size(850, 194),
                ColumnCount = 3,
                RowCount = 2,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            tableMenu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            tableMenu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            tableMenu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.334F));
            tableMenu.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableMenu.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            btnDanhMuc = TaoNutChucNang("▤     Danh mục");
            btnDanhMuc.Click += btnDanhMuc_Click;
            btnPhong = TaoNutChucNang("▣     Phòng - Tiện nghi");
            btnPhong.Click += btnPhong_Click;
            btnDatPhong = TaoNutChucNang("●     Đặt / Nhận phòng");
            btnDatPhong.Click += btnDatPhong_Click;
            btnDichVu = TaoNutChucNang("⚙     Sử dụng dịch vụ");
            btnDichVu.Click += btnDichVu_Click;
            btnTraPhong = TaoNutChucNang("$      Trả phòng - Thanh toán");
            btnTraPhong.Click += btnTraPhong_Click;
            btnThongKe = TaoNutChucNang("▥     Thống kê");
            btnThongKe.Click += btnThongKe_Click;

            tableMenu.Controls.Add(btnDanhMuc, 0, 0);
            tableMenu.Controls.Add(btnPhong, 1, 0);
            tableMenu.Controls.Add(btnDatPhong, 2, 0);
            tableMenu.Controls.Add(btnDichVu, 0, 1);
            tableMenu.Controls.Add(btnTraPhong, 1, 1);
            tableMenu.Controls.Add(btnThongKe, 2, 1);

            btnDangXuat = TaoNutChucNang("↪     Thoát");
            btnDangXuat.Name = "btnDangXuat";
            btnDangXuat.Dock = DockStyle.None;
            btnDangXuat.Margin = new Padding(0);
            btnDangXuat.Size = new Size(275, 66);
            btnDangXuat.Location = new Point(313, 358);
            btnDangXuat.Anchor = AnchorStyles.Top;
            btnDangXuat.ForeColor = Color.FromArgb(144, 48, 48);
            btnDangXuat.Click += btnDangXuat_Click;

            var lblGhiChu = new Label
            {
                Text = "Chọn chức năng cần thực hiện",
                AutoSize = false,
                Size = new Size(400, 24),
                Location = new Point(250, 458),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 9F)
            };

            Controls.Add(lblGhiChu);
            Controls.Add(btnDangXuat);
            Controls.Add(tableMenu);
            Controls.Add(pnlHeader);
        }
    }
}

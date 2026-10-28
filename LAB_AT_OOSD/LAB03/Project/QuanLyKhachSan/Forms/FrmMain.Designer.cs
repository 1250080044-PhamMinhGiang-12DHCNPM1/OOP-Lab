using System.Drawing;
using System.Drawing.Drawing2D;
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

        private Button TaoNutChucNang(string text, string loaiBieuTuong)
        {
            var button = new Button
            {
                Text = text,
                Image = TaoBieuTuong(loaiBieuTuong),
                Dock = DockStyle.Fill,
                Margin = new Padding(9),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(35, 45, 58),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10.5F),
                ImageAlign = ContentAlignment.MiddleLeft,
                TextAlign = ContentAlignment.MiddleCenter,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Padding = new Padding(20, 0, 12, 0),
                Cursor = Cursors.Hand,
                UseVisualStyleBackColor = false
            };
            button.FlatAppearance.BorderColor = Color.FromArgb(155, 164, 175);
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(231, 241, 252);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(211, 230, 249);
            return button;
        }

        private Bitmap TaoBieuTuong(string loai)
        {
            var image = new Bitmap(40, 40);
            using (var g = Graphics.FromImage(image))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                if (loai == "danhmuc")
                {
                    using (var pen = new Pen(Color.FromArgb(42, 105, 168), 2F))
                    using (var paper = new SolidBrush(Color.FromArgb(231, 242, 253)))
                    {
                        g.FillRectangle(paper, 10, 9, 22, 26);
                        g.DrawRectangle(pen, 10, 9, 22, 26);
                        g.DrawLine(pen, 15, 17, 27, 17);
                        g.DrawLine(pen, 15, 23, 27, 23);
                        g.DrawLine(pen, 15, 29, 24, 29);
                        g.DrawRectangle(pen, 16, 5, 10, 7);
                    }
                }
                else if (loai == "phong")
                {
                    using (var frame = new Pen(Color.FromArgb(166, 92, 40), 3F))
                    using (var blanket = new SolidBrush(Color.FromArgb(55, 139, 214)))
                    using (var pillow = new SolidBrush(Color.FromArgb(255, 193, 76)))
                    {
                        g.FillRectangle(blanket, 10, 18, 25, 12);
                        g.FillRectangle(pillow, 11, 14, 10, 7);
                        g.DrawLine(frame, 8, 11, 8, 34);
                        g.DrawLine(frame, 8, 30, 36, 30);
                        g.DrawLine(frame, 34, 19, 34, 34);
                    }
                }
                else if (loai == "datphong")
                {
                    using (var gold = new SolidBrush(Color.FromArgb(247, 181, 34)))
                    using (var pen = new Pen(Color.FromArgb(183, 119, 9), 2F))
                    {
                        g.FillEllipse(gold, 6, 7, 17, 17);
                        g.DrawEllipse(pen, 6, 7, 17, 17);
                        g.FillRectangle(gold, 18, 17, 17, 7);
                        g.DrawLine(pen, 19, 18, 34, 18);
                        g.DrawLine(pen, 30, 22, 30, 27);
                        g.DrawLine(pen, 34, 22, 34, 26);
                        g.FillEllipse(Brushes.White, 11, 12, 7, 7);
                    }
                }
                else if (loai == "dichvu")
                {
                    using (var pen = new Pen(Color.FromArgb(74, 112, 151), 4F))
                    using (var fill = new SolidBrush(Color.FromArgb(221, 234, 246)))
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            double a = i * System.Math.PI / 4D;
                            int x1 = 20 + (int)(10 * System.Math.Cos(a));
                            int y1 = 20 + (int)(10 * System.Math.Sin(a));
                            int x2 = 20 + (int)(15 * System.Math.Cos(a));
                            int y2 = 20 + (int)(15 * System.Math.Sin(a));
                            g.DrawLine(pen, x1, y1, x2, y2);
                        }
                        g.FillEllipse(fill, 9, 9, 22, 22);
                        g.DrawEllipse(pen, 9, 9, 22, 22);
                        g.FillEllipse(Brushes.White, 16, 16, 8, 8);
                    }
                }
                else if (loai == "thanhtoan")
                {
                    using (var green = new SolidBrush(Color.FromArgb(55, 157, 82)))
                    using (var hand = new Pen(Color.FromArgb(225, 153, 45), 4F))
                    using (var moneyFont = new Font("Segoe UI Semibold", 9F))
                    {
                        g.FillRectangle(green, 13, 7, 22, 15);
                        g.DrawRectangle(Pens.ForestGreen, 13, 7, 22, 15);
                        g.DrawString("$", moneyFont, Brushes.White, 20, 7);
                        g.DrawArc(hand, 6, 19, 27, 14, 15, 155);
                        g.DrawLine(hand, 8, 28, 4, 24);
                    }
                }
                else if (loai == "thongke")
                {
                    g.FillRectangle(Brushes.DodgerBlue, 7, 23, 7, 12);
                    g.FillRectangle(Brushes.Orange, 17, 15, 7, 20);
                    g.FillRectangle(Brushes.MediumSeaGreen, 27, 8, 7, 27);
                    using (var pen = new Pen(Color.FromArgb(75, 88, 103), 1.5F))
                    {
                        g.DrawLine(pen, 5, 35, 36, 35);
                        g.DrawLine(pen, 5, 5, 5, 35);
                    }
                }
                else if (loai == "thoat")
                {
                    using (var door = new SolidBrush(Color.FromArgb(170, 99, 55)))
                    using (var arrow = new Pen(Color.FromArgb(48, 154, 88), 4F))
                    {
                        g.FillRectangle(door, 7, 6, 16, 29);
                        g.DrawRectangle(Pens.SaddleBrown, 7, 6, 16, 29);
                        g.FillEllipse(Brushes.Gold, 18, 20, 3, 3);
                        g.DrawLine(arrow, 20, 20, 35, 20);
                        g.DrawLine(arrow, 35, 20, 29, 14);
                        g.DrawLine(arrow, 35, 20, 29, 26);
                    }
                }
            }
            return image;
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

            btnDanhMuc = TaoNutChucNang("Danh mục", "danhmuc");
            btnDanhMuc.Click += btnDanhMuc_Click;
            btnPhong = TaoNutChucNang("Phòng - Tiện nghi", "phong");
            btnPhong.Click += btnPhong_Click;
            btnDatPhong = TaoNutChucNang("Đặt / Nhận phòng", "datphong");
            btnDatPhong.Click += btnDatPhong_Click;
            btnDichVu = TaoNutChucNang("Sử dụng dịch vụ", "dichvu");
            btnDichVu.Click += btnDichVu_Click;
            btnTraPhong = TaoNutChucNang("Trả phòng - Thanh toán", "thanhtoan");
            btnTraPhong.Click += btnTraPhong_Click;
            btnThongKe = TaoNutChucNang("Thống kê", "thongke");
            btnThongKe.Click += btnThongKe_Click;

            tableMenu.Controls.Add(btnDanhMuc, 0, 0);
            tableMenu.Controls.Add(btnPhong, 1, 0);
            tableMenu.Controls.Add(btnDatPhong, 2, 0);
            tableMenu.Controls.Add(btnDichVu, 0, 1);
            tableMenu.Controls.Add(btnTraPhong, 1, 1);
            tableMenu.Controls.Add(btnThongKe, 2, 1);

            btnDangXuat = TaoNutChucNang("Thoát", "thoat");
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

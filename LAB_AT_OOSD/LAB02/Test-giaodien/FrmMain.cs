using System;
using System.Drawing;
using System.Windows.Forms;
using Test_giaodien.Forms;

namespace Test_giaodien
{
    public class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            Text = "Quản lý thư viện";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(760, 480);
            Font = new Font("Segoe UI", 10F);
            BackColor = Color.FromArgb(245, 248, 252);

            Label title = new Label
            {
                Text = "HỆ THỐNG QUẢN LÝ THƯ VIỆN",
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 85,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(31, 78, 121)
            };
            Label subtitle = new Label
            {
                Text = "Dữ liệu được kết nối từ SQL Server",
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 34,
                TextAlign = ContentAlignment.TopCenter,
                ForeColor = Color.DimGray
            };
            FlowLayoutPanel menu = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(85, 20, 85, 20),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true
            };

            menu.Controls.Add(CreateMenuButton("Danh mục", (s, e) => OpenForm(new FrmDanhMuc())));
            menu.Controls.Add(CreateMenuButton("Quản lý sách", (s, e) => OpenForm(new FrmSach())));
            menu.Controls.Add(CreateMenuButton("Độc giả và thẻ", (s, e) => OpenForm(new FrmDocGia())));
            menu.Controls.Add(CreateMenuButton("Mượn - trả sách", (s, e) => OpenForm(new FrmMuonTra())));
            menu.Controls.Add(CreateMenuButton("Thống kê", (s, e) => OpenForm(new FrmThongKe())));
            menu.Controls.Add(CreateMenuButton("Thoát", btnThoat_Click, Color.FromArgb(192, 57, 43)));

            Controls.Add(menu);
            Controls.Add(subtitle);
            Controls.Add(title);
        }

        private Button CreateMenuButton(string text, EventHandler clickHandler, Color? backColor = null)
        {
            Button button = new Button
            {
                Text = text,
                Size = new Size(270, 76),
                Margin = new Padding(18, 12, 18, 12),
                FlatStyle = FlatStyle.Flat,
                BackColor = backColor ?? Color.FromArgb(49, 112, 180),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = 0;
            button.Click += clickHandler;
            return button;
        }

        private void OpenForm(Form form)
        {
            using (form)
            {
                form.ShowDialog(this);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát ứng dụng không?", "Xác nhận thoát",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
        }
    }
}

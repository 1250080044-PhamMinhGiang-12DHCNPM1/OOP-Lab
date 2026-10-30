using System;
using System.Drawing;
using System.Windows.Forms;
using ChoThueVanPhongCaoOc.Data;

namespace ChoThueVanPhongCaoOc.Forms
{
    internal sealed class FrmMain : Form
    {
        internal FrmMain()
        {
            Text = "Quản lý cho thuê văn phòng cao ốc"; Width = 1050; Height = 680; StartPosition = FormStartPosition.CenterScreen;
            var title = new Label { Text = "QUẢN LÝ CHO THUÊ VĂN PHÒNG CAO ỐC", Dock = DockStyle.Top, Height = 70, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 18, FontStyle.Bold), BackColor = Color.FromArgb(31, 78, 121), ForeColor = Color.White };
            var menu = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 150, Padding = new Padding(20), AutoScroll = true };
            Add(menu, "Tầng và phòng", () => new FrmTangPhong()); Add(menu, "Khách hàng", () => new FrmKhachHang());
            Add(menu, "Lập hợp đồng", () => new FrmHopDong()); Add(menu, "Thanh toán", () => new FrmThanhToan());
            Add(menu, "Dịch vụ", () => new FrmDichVu()); Add(menu, "Gia hạn", () => new FrmGiaHan());
            Add(menu, "Nhân viên", () => new FrmNhanVien()); Add(menu, "Thẻ giữ xe", () => new FrmTheGiuXe()); Add(menu, "Kiểm tra phòng", () => new FrmKiemTra());
            var grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White };
            Controls.Add(grid); Controls.Add(menu); Controls.Add(title);
            Shown += (s, e) => { try { grid.DataSource = Db.Query("SELECT TOP 100 * FROM dbo.vw_CongNo ORDER BY HanThanhToan, MaKhoanThu"); } catch (Exception ex) { MessageBox.Show("Chưa kết nối được database. Hãy chạy Database_LAB08.sql trước.\n\n" + ex.Message, "Kết nối SQL Server"); } };
        }

        private static void Add(Control parent, string text, Func<Form> create)
        {
            var b = new Button { Text = text, Width = 150, Height = 45, Margin = new Padding(6), BackColor = Color.White };
            b.Click += (s, e) => create().ShowDialog(); parent.Controls.Add(b);
        }
    }
}

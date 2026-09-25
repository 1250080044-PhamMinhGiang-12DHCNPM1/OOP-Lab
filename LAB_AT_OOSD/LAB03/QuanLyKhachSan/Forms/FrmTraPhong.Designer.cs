using System.Drawing;
using System.Windows.Forms;
namespace QuanLyKhachSan.Forms
{
    partial class FrmTraPhong
    {
        private System.ComponentModel.IContainer components = null; private Button btnInHoaDon;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container(); UiTheme.StyleForm(this, "Trả phòng và thanh toán"); ClientSize = new Size(1200, 740);
            var header = new Panel { Dock = DockStyle.Top, Height = 80, BackColor = Color.White, Padding = new Padding(24, 18, 24, 10) }; header.Controls.Add(UiTheme.Heading("Trả phòng và thanh toán", 22F));
            var tabs = new TabControl { Dock = DockStyle.Fill, Padding = new Point(16, 7) };
            var damage = new TabPage("Kiểm tra - Đền bù") { BackColor = UiTheme.Background, Padding = new Padding(12) }; var dRoot = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3 }; dRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 80)); dRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); dRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 145));
            var dFilter = new FlowLayoutPanel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(8) }; dFilter.Controls.Add(UiTheme.Field("Phiếu đang ở", UiTheme.Combo("cboPhieu", "DP20260925001"), 250)); dFilter.Controls.Add(UiTheme.Field("Phòng", UiTheme.Combo("cboPhong", "A101"))); dRoot.Controls.Add(dFilter, 0, 0);
            dRoot.Controls.Add(UiTheme.Grid("Mã tiện nghi", "Tên loại", "Tình trạng trước", "Mức độ thiệt hại", "Tiền đền bù"), 0, 1); var dEdit = new FlowLayoutPanel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(8) };
            dEdit.Controls.Add(UiTheme.Field("Tiện nghi", UiTheme.Combo("cboTienNghi", "TV01", "TL01"))); dEdit.Controls.Add(UiTheme.Field("Mức độ", UiTheme.Combo("cboMucDo", "Hư hỏng nhẹ", "Mất"))); dEdit.Controls.Add(UiTheme.Field("Số tiền", new NumericUpDown { Width = 180, Maximum = 100000000, Increment = 100000 })); dEdit.Controls.Add(UiTheme.Button("Thêm đền bù", false)); dEdit.Controls.Add(UiTheme.Button("Lập phiếu", true)); dRoot.Controls.Add(dEdit, 0, 2); damage.Controls.Add(dRoot);
            var invoice = new TabPage("Hóa đơn - Thanh toán") { BackColor = UiTheme.Background, Padding = new Padding(12) }; var iRoot = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3 }; iRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 110)); iRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); iRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 150));
            var summary = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(4) }; summary.Controls.Add(UiTheme.Card("Tiền phòng", "0 đ", UiTheme.Blue)); summary.Controls.Add(UiTheme.Card("Tiền dịch vụ", "0 đ", Color.FromArgb(124, 58, 237))); summary.Controls.Add(UiTheme.Card("Đã thanh toán", "0 đ", Color.FromArgb(35, 165, 92))); summary.Controls.Add(UiTheme.Card("Còn thiếu", "0 đ", Color.FromArgb(220, 68, 68))); iRoot.Controls.Add(summary, 0, 0);
            iRoot.Controls.Add(UiTheme.Grid("Số hóa đơn", "Phiếu đặt", "Số đêm", "Tiền phòng", "Tiền dịch vụ", "Tổng tiền", "Trạng thái"), 0, 1); var pay = new FlowLayoutPanel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(8) };
            pay.Controls.Add(UiTheme.Field("Mã thanh toán", UiTheme.Input("txtMaTT"))); pay.Controls.Add(UiTheme.Field("Hình thức", UiTheme.Combo("cboHinhThuc", "Tiền mặt", "Chuyển khoản", "Thẻ", "Ví điện tử"))); pay.Controls.Add(UiTheme.Field("Số tiền", new NumericUpDown { Width = 180, Maximum = 1000000000, Increment = 100000 })); pay.Controls.Add(UiTheme.Button("Thanh toán", true));
            btnInHoaDon = UiTheme.Button("Xem trước in", false); btnInHoaDon.Width = 125; btnInHoaDon.Click += btnInHoaDon_Click; pay.Controls.Add(btnInHoaDon); pay.Controls.Add(UiTheme.Button("Trả phòng", false)); iRoot.Controls.Add(pay, 0, 2); invoice.Controls.Add(iRoot);
            tabs.TabPages.Add(damage); tabs.TabPages.Add(invoice); Controls.Add(tabs); Controls.Add(header);
        }
    }
}

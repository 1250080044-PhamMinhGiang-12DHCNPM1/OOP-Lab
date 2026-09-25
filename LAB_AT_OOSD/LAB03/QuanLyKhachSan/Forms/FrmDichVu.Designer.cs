using System.Drawing;
using System.Windows.Forms;
namespace QuanLyKhachSan.Forms
{
    partial class FrmDichVu
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container(); UiTheme.StyleForm(this, "Ghi nhận dịch vụ"); ClientSize = new Size(1100, 700);
            var header = new Panel { Dock = DockStyle.Top, Height = 80, BackColor = Color.White, Padding = new Padding(24, 18, 24, 10) }; header.Controls.Add(UiTheme.Heading("Ghi nhận dịch vụ", 22F));
            var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(18), RowCount = 3 }; root.RowStyles.Add(new RowStyle(SizeType.Absolute, 95)); root.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); root.RowStyles.Add(new RowStyle(SizeType.Absolute, 135));
            var booking = new FlowLayoutPanel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(10) }; booking.Controls.Add(UiTheme.Field("Phiếu đang ở", UiTheme.Combo("cboPhieu", "DP20260925001 - Phòng A101"), 280)); booking.Controls.Add(UiTheme.Field("Phòng", UiTheme.Combo("cboPhong", "A101"))); booking.Controls.Add(UiTheme.Field("Ngày sử dụng", new DateTimePicker { Width = 180, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" })); root.Controls.Add(booking, 0, 0);
            var grid = UiTheme.Grid("Mã dịch vụ", "Tên dịch vụ", "Số lượng", "Đơn giá", "Thành tiền", "Ngày sử dụng"); grid.Rows.Add("DV01", "Ăn sáng", "2", "120.000", "240.000", System.DateTime.Today.ToString("dd/MM/yyyy")); root.Controls.Add(grid, 0, 1);
            var editor = new FlowLayoutPanel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(10) }; editor.Controls.Add(UiTheme.Field("Dịch vụ", UiTheme.Combo("cboDichVu", "DV01 - Ăn sáng", "DV02 - Tắm hơi", "DV03 - Karaoke"), 250)); editor.Controls.Add(UiTheme.Field("Số lượng", new NumericUpDown { Width = 180, Minimum = 1, Maximum = 100, Value = 1 })); editor.Controls.Add(UiTheme.Field("Đơn giá", UiTheme.Input("txtDonGia"))); editor.Controls.Add(UiTheme.Button("Ghi nhận", true)); editor.Controls.Add(UiTheme.Button("Làm mới", false)); root.Controls.Add(editor, 0, 2);
            Controls.Add(root); Controls.Add(header);
        }
    }
}

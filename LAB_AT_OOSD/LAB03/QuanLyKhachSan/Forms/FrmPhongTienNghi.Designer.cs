using System.Drawing;
using System.Windows.Forms;
namespace QuanLyKhachSan.Forms
{
    partial class FrmPhongTienNghi
    {
        private System.ComponentModel.IContainer components = null; private TabControl tabs;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container(); UiTheme.StyleForm(this, "Phòng và tiện nghi"); ClientSize = new Size(1180, 730);
            var header = new Panel { Dock = DockStyle.Top, Height = 82, BackColor = Color.White, Padding = new Padding(24, 18, 24, 10) };
            var title = UiTheme.Heading("Phòng và tiện nghi", 22F); var note = new Label { Text = "Quản lý phòng, thiết bị và lịch sử lắp đặt", AutoSize = true, ForeColor = Color.DimGray, Location = new Point(27, 52) }; header.Controls.Add(title); header.Controls.Add(note);
            tabs = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10F), Padding = new Point(16, 7) };
            tabs.TabPages.Add(UiTheme.CrudPage("Phòng", new[] { "Số phòng", "Khu vực", "Sức chứa", "Đơn giá", "Trạng thái" }, new[] { "Số phòng", "Khu vực", "Sức chứa tối đa", "Đơn giá ngày", "Trạng thái" }));
            tabs.TabPages.Add(UiTheme.CrudPage("Tiện nghi", new[] { "Mã tiện nghi", "Loại", "Số thứ tự", "Tình trạng" }, new[] { "Mã tiện nghi", "Loại tiện nghi", "Số thứ tự", "Tình trạng" }));
            var lapDat = new TabPage("Lắp đặt - Luân chuyển") { BackColor = UiTheme.Background, Padding = new Padding(12) };
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1 }; layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 155));
            var grid = UiTheme.Grid("Số phiếu", "Mã tiện nghi", "Số phòng", "Ngày lắp", "Tình trạng", "Nhân viên"); layout.Controls.Add(grid, 0, 0);
            var editor = new FlowLayoutPanel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(8), AutoScroll = true };
            editor.Controls.Add(UiTheme.Field("Số phiếu", UiTheme.Input("txtSoPhieu"))); editor.Controls.Add(UiTheme.Field("Tiện nghi", UiTheme.Combo("cboTienNghi", "TV01", "TV02", "TL01", "DT01")));
            editor.Controls.Add(UiTheme.Field("Phòng", UiTheme.Combo("cboPhong", "A101", "A102", "B201"))); editor.Controls.Add(UiTheme.Field("Ngày lắp", new DateTimePicker { Width = 180, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" }));
            editor.Controls.Add(UiTheme.Field("Tình trạng", UiTheme.Combo("cboTinhTrang", "Tốt", "Cần bảo trì", "Hỏng"))); editor.Controls.Add(UiTheme.Button("Lập phiếu", true)); editor.Controls.Add(UiTheme.Button("Làm mới", false));
            layout.Controls.Add(editor, 0, 1); lapDat.Controls.Add(layout); tabs.TabPages.Add(lapDat);
            Controls.Add(tabs); Controls.Add(header);
        }
    }
}

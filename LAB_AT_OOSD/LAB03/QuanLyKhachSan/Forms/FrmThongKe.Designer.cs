using System.Drawing;
using System.Windows.Forms;
namespace QuanLyKhachSan.Forms
{
    partial class FrmThongKe
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container(); UiTheme.StyleForm(this, "Thống kê"); ClientSize = new Size(1180, 730);
            var header = new Panel { Dock = DockStyle.Top, Height = 80, BackColor = Color.White, Padding = new Padding(24, 18, 24, 10) }; header.Controls.Add(UiTheme.Heading("Thống kê hoạt động", 22F));
            var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(18), RowCount = 4 }; root.RowStyles.Add(new RowStyle(SizeType.Absolute, 85)); root.RowStyles.Add(new RowStyle(SizeType.Absolute, 125)); root.RowStyles.Add(new RowStyle(SizeType.Percent, 55)); root.RowStyles.Add(new RowStyle(SizeType.Percent, 45));
            var filter = new FlowLayoutPanel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(8) }; filter.Controls.Add(UiTheme.Field("Từ ngày", new DateTimePicker { Width = 180, Value = System.DateTime.Today.AddDays(-30), Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" })); filter.Controls.Add(UiTheme.Field("Đến ngày", new DateTimePicker { Width = 180, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" })); filter.Controls.Add(UiTheme.Button("Xem thống kê", true)); root.Controls.Add(filter, 0, 0);
            var cards = new FlowLayoutPanel { Dock = DockStyle.Fill }; cards.Controls.Add(UiTheme.Card("Phiếu đặt", "0", UiTheme.Blue)); cards.Controls.Add(UiTheme.Card("Khách đang ở", "0", Color.FromArgb(245, 158, 11))); cards.Controls.Add(UiTheme.Card("Doanh thu", "0 đ", Color.FromArgb(35, 165, 92))); cards.Controls.Add(UiTheme.Card("Tiền đền bù", "0 đ", Color.FromArgb(220, 68, 68))); root.Controls.Add(cards, 0, 1);
            var roomGroup = new GroupBox { Text = "Tổng hợp đặt phòng và hóa đơn", Dock = DockStyle.Fill, Padding = new Padding(10), Font = new Font("Segoe UI Semibold", 10F) }; roomGroup.Controls.Add(UiTheme.Grid("Ngày", "Số phiếu đặt", "Số hóa đơn", "Doanh thu", "Đền bù")); root.Controls.Add(roomGroup, 0, 2);
            var serviceGroup = new GroupBox { Text = "Dịch vụ được sử dụng", Dock = DockStyle.Fill, Padding = new Padding(10), Font = new Font("Segoe UI Semibold", 10F) }; serviceGroup.Controls.Add(UiTheme.Grid("Mã dịch vụ", "Tên dịch vụ", "Tổng số lượng", "Tổng tiền")); root.Controls.Add(serviceGroup, 0, 3);
            Controls.Add(root); Controls.Add(header);
        }
    }
}

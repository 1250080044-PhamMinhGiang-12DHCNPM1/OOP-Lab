using System.Drawing;
using System.Windows.Forms;
namespace QuanLyKhachSan.Forms
{
    partial class FrmDatPhong
    {
        private System.ComponentModel.IContainer components = null; private Label lblKetQua; private Button btnTimPhong, btnLapPhieu; private TabControl tabs;
        protected override void Dispose(bool disposing) { if (disposing && components != null) components.Dispose(); base.Dispose(disposing); }
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container(); UiTheme.StyleForm(this, "Đặt và nhận phòng"); ClientSize = new Size(1240, 760);
            var header = new Panel { Dock = DockStyle.Top, Height = 78, BackColor = Color.White, Padding = new Padding(24, 18, 24, 10) }; header.Controls.Add(UiTheme.Heading("Đặt và nhận phòng", 22F));
            tabs = new TabControl { Dock = DockStyle.Fill, Padding = new Point(16, 7) };
            var booking = new TabPage("Đặt phòng") { BackColor = UiTheme.Background, Padding = new Padding(12) };
            var root = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 4, ColumnCount = 1 };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 100)); root.RowStyles.Add(new RowStyle(SizeType.Percent, 52)); root.RowStyles.Add(new RowStyle(SizeType.Percent, 28)); root.RowStyles.Add(new RowStyle(SizeType.Absolute, 128));
            var filter = new FlowLayoutPanel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(10) };
            filter.Controls.Add(UiTheme.Field("Ngày nhận", new DateTimePicker { Name = "dtNhan", Width = 180, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" })); filter.Controls.Add(UiTheme.Field("Ngày trả", new DateTimePicker { Name = "dtTra", Width = 180, Value = System.DateTime.Today.AddDays(1), Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy" }));
            filter.Controls.Add(UiTheme.Field("Số khách", new NumericUpDown { Name = "numSoKhach", Width = 180, Minimum = 1, Maximum = 20, Value = 2 })); btnTimPhong = UiTheme.Button("Tìm phòng trống", true); btnTimPhong.Width = 145; btnTimPhong.Click += btnTimPhong_Click; filter.Controls.Add(btnTimPhong);
            lblKetQua = new Label { Text = "Nhập điều kiện để tìm phòng", AutoSize = true, ForeColor = UiTheme.Blue, Margin = new Padding(10, 15, 0, 0) }; filter.Controls.Add(lblKetQua); root.Controls.Add(filter, 0, 0);
            var available = UiTheme.Grid("Số phòng", "Khu vực", "Sức chứa", "Đơn giá", "Trạng thái"); available.Name = "dgvPhongTrong"; root.Controls.Add(available, 0, 1);
            var selected = UiTheme.Grid("Số phòng", "Số khách", "Đơn giá", "Số đêm", "Tạm tính"); root.Controls.Add(selected, 0, 2);
            var editor = new FlowLayoutPanel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(8), AutoScroll = true };
            editor.Controls.Add(UiTheme.Field("Khách hàng", UiTheme.Combo("cboKhach", "KH001 - Nguyễn Văn An"))); editor.Controls.Add(UiTheme.Field("Kênh đặt", UiTheme.Combo("cboKenh", "Trực tiếp", "Điện thoại", "Website")));
            editor.Controls.Add(UiTheme.Field("Tiền cọc", new NumericUpDown { Name = "numTienCoc", Width = 180, Maximum = 100000000, Increment = 100000 })); btnLapPhieu = UiTheme.Button("Lập phiếu", true); btnLapPhieu.Click += btnLapPhieu_Click; editor.Controls.Add(btnLapPhieu); editor.Controls.Add(UiTheme.Button("Hủy chọn", false)); root.Controls.Add(editor, 0, 3); booking.Controls.Add(root);
            var checkin = new TabPage("Nhận phòng") { BackColor = UiTheme.Background, Padding = new Padding(12) }; var ci = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2 }; ci.RowStyles.Add(new RowStyle(SizeType.Percent, 65)); ci.RowStyles.Add(new RowStyle(SizeType.Percent, 35));
            var bookingGrid = UiTheme.Grid("Số phiếu", "Khách đặt", "Ngày nhận", "Ngày trả", "Phòng", "Số khách", "Trạng thái"); bookingGrid.Name = "dgvPhieuDat"; ci.Controls.Add(bookingGrid, 0, 0); var ciEdit = new FlowLayoutPanel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12) };
            ciEdit.Controls.Add(UiTheme.Field("Họ tên người ở", UiTheme.Input("txtNguoiO"))); ciEdit.Controls.Add(UiTheme.Field("CCCD", UiTheme.Input("txtCCCD"))); ciEdit.Controls.Add(UiTheme.Field("Quốc tịch", UiTheme.Input("txtQuocTich"))); var addGuest = UiTheme.Button("Thêm người ở", false); addGuest.Name = "btnThemNguoi"; ciEdit.Controls.Add(addGuest); var receive = UiTheme.Button("Nhận phòng", true); receive.Name = "btnNhanPhong"; ciEdit.Controls.Add(receive); ci.Controls.Add(ciEdit, 0, 1); checkin.Controls.Add(ci);
            var customer = UiTheme.CrudPage("Khách hàng", new[] { "Mã khách", "Họ tên", "CCCD", "Quốc tịch", "Điện thoại" }, new[] { "Mã khách", "Họ tên", "CCCD", "Quốc tịch", "Điện thoại" });
            tabs.TabPages.Add(booking); tabs.TabPages.Add(checkin); tabs.TabPages.Add(customer); Controls.Add(tabs); Controls.Add(header);
        }
    }
}

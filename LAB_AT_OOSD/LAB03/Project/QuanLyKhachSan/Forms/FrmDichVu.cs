using System;
using System.Data;
using System.Windows.Forms;
using QuanLyKhachSan.Services;

namespace QuanLyKhachSan.Forms
{
    public partial class FrmDichVu : Form
    {
        private readonly NghiepVuService service = new NghiepVuService();
        public FrmDichVu() { InitializeComponent(); Load += FrmDichVu_Load; }
        private void FrmDichVu_Load(object sender, EventArgs e)
        {
            var booking = UiTheme.Find<ComboBox>(this, "cboPhieu"); booking.DataSource = service.LayPhieuDangO(); booking.DisplayMember = "HienThi"; booking.ValueMember = "SoPhieuDat"; booking.SelectedIndexChanged += (s, a) => ChonPhieu();
            var serviceCombo = UiTheme.Find<ComboBox>(this, "cboDichVu"); serviceCombo.DataSource = service.LayDichVuCombo(); serviceCombo.DisplayMember = "HienThi"; serviceCombo.ValueMember = "MaDV"; serviceCombo.SelectedIndexChanged += (s, a) => ChonDichVu();
            UiTheme.Find<Button>(this, "btnGhiNhan").Click += GhiNhan_Click; UiTheme.Find<Button>(this, "btnTaiLai").Click += (s, a) => Tai(); ChonPhieu(); ChonDichVu();
        }
        private void ChonPhieu() { var combo = UiTheme.Find<ComboBox>(this, "cboPhieu"); var row = combo.SelectedItem as DataRowView; if (row == null) return; var room = UiTheme.Find<ComboBox>(this, "cboPhong"); room.DataSource = new[] { Convert.ToString(row["SoPhong"]) }; Tai(); }
        private void ChonDichVu() { var row = UiTheme.Find<ComboBox>(this, "cboDichVu").SelectedItem as DataRowView; if (row != null) UiTheme.Find<TextBox>(this, "txtDonGia").Text = Convert.ToDecimal(row["DonGia"]).ToString("N0"); }
        private void Tai() { var phieu = UiTheme.Find<ComboBox>(this, "cboPhieu"); var row = phieu.SelectedItem as DataRowView; if (row == null) return; var grid = UiTheme.Find<DataGridView>(this, "dgvDichVu"); grid.Columns.Clear(); grid.AutoGenerateColumns = true; grid.DataSource = service.LayDichVuDaDung(Convert.ToString(row["SoPhieuDat"]), Convert.ToString(row["SoPhong"])); }
        private void GhiNhan_Click(object sender, EventArgs e) { var booking = UiTheme.Find<ComboBox>(this, "cboPhieu").SelectedItem as DataRowView; if (booking == null) { MessageBox.Show("Hiện không có phiếu đang ở."); return; } var r = service.GhiDichVu(Convert.ToString(booking["SoPhieuDat"]), Convert.ToString(booking["SoPhong"]), UiTheme.Find<DateTimePicker>(this, "dtNgaySuDung").Value, Convert.ToString(UiTheme.Find<ComboBox>(this, "cboDichVu").SelectedValue), (int)UiTheme.Find<NumericUpDown>(this, "numSoLuong").Value); MessageBox.Show(r.ThongBao); if (r.ThanhCong) Tai(); }
    }
}

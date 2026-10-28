using System;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Data;
using QuanLyKhachSan.Services;
namespace QuanLyKhachSan.Forms
{
    public partial class FrmTraPhong : Form
    {
        private readonly PrintDocument hoaDon = new PrintDocument();
        private readonly NghiepVuService service = new NghiepVuService(); private string noiDungIn = "Chưa chọn hóa đơn";
        public FrmTraPhong() { InitializeComponent(); Load += FrmTraPhong_Load; hoaDon.PrintPage += (s, e) => e.Graphics.DrawString("HÓA ĐƠN KHÁCH SẠN\n\n" + noiDungIn, new System.Drawing.Font("Segoe UI", 14), System.Drawing.Brushes.Black, 80, 80); }
        private void FrmTraPhong_Load(object sender, EventArgs e)
        {
            var booking = UiTheme.Find<ComboBox>(tabs.TabPages[0], "cboPhieu"); booking.DataSource = service.LayPhieuDangO(); booking.DisplayMember = "HienThi"; booking.ValueMember = "SoPhieuDat"; booking.SelectedIndexChanged += (s, a) => ChonPhieu();
            UiTheme.Find<Button>(this, "btnLapDenBu").Click += LapDenBu_Click; UiTheme.Find<Button>(this, "btnLapHoaDon").Click += LapHoaDon_Click; UiTheme.Find<Button>(this, "btnThanhToan").Click += ThanhToan_Click; UiTheme.Find<Button>(this, "btnTraPhong").Click += TraPhong_Click;
            UiTheme.Find<DataGridView>(this, "dgvHoaDon").SelectionChanged += (s, a) => ChonHoaDon(); ChonPhieu(); TaiHoaDon();
        }
        private DataRowView PhieuDangChon() { return UiTheme.Find<ComboBox>(tabs.TabPages[0], "cboPhieu").SelectedItem as DataRowView; }
        private void ChonPhieu()
        {
            var row = PhieuDangChon(); if (row == null) return; string room = Convert.ToString(row["SoPhong"]); var roomCombo = UiTheme.Find<ComboBox>(tabs.TabPages[0], "cboPhong"); roomCombo.DataSource = new[] { room };
            var data = service.LayTienNghiPhong(room); var grid = UiTheme.Find<DataGridView>(this, "dgvTienNghi"); grid.Columns.Clear(); grid.AutoGenerateColumns = true; grid.DataSource = data;
            var combo = UiTheme.Find<ComboBox>(this, "cboTienNghi"); combo.DataSource = data.Copy(); combo.DisplayMember = "Mã tiện nghi"; combo.ValueMember = "Mã tiện nghi";
        }
        private void LapDenBu_Click(object sender, EventArgs e) { var row = PhieuDangChon(); if (row == null) return; var r = service.LapDenBu(Convert.ToString(row["SoPhieuDat"]), Convert.ToString(row["SoPhong"]), Convert.ToString(UiTheme.Find<ComboBox>(this, "cboTienNghi").SelectedValue), UiTheme.Find<ComboBox>(this, "cboMucDo").Text, UiTheme.Find<NumericUpDown>(this, "numDenBu").Value); MessageBox.Show(r.ThongBao); }
        private void LapHoaDon_Click(object sender, EventArgs e) { var row = PhieuDangChon(); if (row == null) { MessageBox.Show("Không có phiếu đang ở."); return; } var r = service.LapHoaDon(Convert.ToString(row["SoPhieuDat"])); MessageBox.Show(r.ThongBao); if (r.ThanhCong) TaiHoaDon(); }
        private void TaiHoaDon() { var grid = UiTheme.Find<DataGridView>(this, "dgvHoaDon"); grid.Columns.Clear(); grid.AutoGenerateColumns = true; grid.DataSource = service.LayHoaDon(); }
        private void ChonHoaDon() { var grid = UiTheme.Find<DataGridView>(this, "dgvHoaDon"); if (grid.CurrentRow == null || grid.CurrentRow.DataBoundItem == null) return; decimal tong = Convert.ToDecimal(grid.CurrentRow.Cells["Tổng tiền"].Value); decimal da = Convert.ToDecimal(grid.CurrentRow.Cells["Đã thanh toán"].Value); SetCard("Tiền phòng", Convert.ToDecimal(grid.CurrentRow.Cells["Tiền phòng"].Value).ToString("N0") + " đ"); SetCard("Tiền dịch vụ", Convert.ToDecimal(grid.CurrentRow.Cells["Tiền dịch vụ"].Value).ToString("N0") + " đ"); SetCard("Đã thanh toán", da.ToString("N0") + " đ"); SetCard("Còn thiếu", (tong - da).ToString("N0") + " đ"); noiDungIn = "Số hóa đơn: " + grid.CurrentRow.Cells["Số hóa đơn"].Value + "\nPhiếu đặt: " + grid.CurrentRow.Cells["Phiếu đặt"].Value + "\nTổng tiền: " + tong.ToString("N0") + " VNĐ\nĐã thanh toán: " + da.ToString("N0") + " VNĐ"; }
        private void SetCard(string name, string value) { foreach (Panel panel in UiTheme.FindAll<Panel>(this)) if (Convert.ToString(panel.Tag) == name) { var label = UiTheme.Find<Label>(panel, "lblCardValue"); if (label != null) label.Text = value; } }
        private string SoHoaDonChon() { var grid = UiTheme.Find<DataGridView>(this, "dgvHoaDon"); return grid.CurrentRow == null ? "" : Convert.ToString(grid.CurrentRow.Cells["Số hóa đơn"].Value); }
        private void ThanhToan_Click(object sender, EventArgs e) { var r = service.ThanhToan(SoHoaDonChon(), UiTheme.Find<ComboBox>(this, "cboHinhThuc").Text, UiTheme.Find<NumericUpDown>(this, "numTienTT").Value); MessageBox.Show(r.ThongBao); if (r.ThanhCong) TaiHoaDon(); }
        private void TraPhong_Click(object sender, EventArgs e) { var r = service.TraPhong(SoHoaDonChon()); MessageBox.Show(r.ThongBao); if (r.ThanhCong) { TaiHoaDon(); var combo = UiTheme.Find<ComboBox>(tabs.TabPages[0], "cboPhieu"); combo.DataSource = service.LayPhieuDangO(); } }
        private void btnInHoaDon_Click(object sender, EventArgs e) { using (var preview = new PrintPreviewDialog { Document = hoaDon, Width = 900, Height = 650 }) preview.ShowDialog(this); }
    }
}

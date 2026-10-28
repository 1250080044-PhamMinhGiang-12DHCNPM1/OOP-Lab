using System;
using System.Windows.Forms;
using System.Linq;
using QuanLyKhachSan.Services;
namespace QuanLyKhachSan.Forms
{
    public partial class FrmDatPhong : Form
    {
        private readonly NghiepVuService service = new NghiepVuService();
        public FrmDatPhong() { InitializeComponent(); Load += FrmDatPhong_Load; }
        private void FrmDatPhong_Load(object sender, EventArgs e)
        {
            TaiKhachCombo(); TaiPhieuDat(); KhoiTaoKhachHang(); btnTimPhong_Click(null, EventArgs.Empty);
            UiTheme.Find<Button>(this, "btnThemNguoi").Click += ThemNguoi_Click; UiTheme.Find<Button>(this, "btnNhanPhong").Click += NhanPhong_Click;
        }
        private void TaiKhachCombo() { var combo = UiTheme.Find<ComboBox>(this, "cboKhach"); combo.DataSource = service.LayKhachCombo(); combo.DisplayMember = "HienThi"; combo.ValueMember = "MaKhach"; }
        private void TaiPhieuDat() { var grid = UiTheme.Find<DataGridView>(this, "dgvPhieuDat"); grid.Columns.Clear(); grid.AutoGenerateColumns = true; grid.DataSource = service.LayPhieuDat("Đã đặt"); }
        private void btnTimPhong_Click(object sender, EventArgs e)
        {
            try { var grid = UiTheme.Find<DataGridView>(this, "dgvPhongTrong"); grid.Columns.Clear(); grid.AutoGenerateColumns = true; grid.DataSource = service.TimPhongTrong(UiTheme.Find<DateTimePicker>(this, "dtNhan").Value, UiTheme.Find<DateTimePicker>(this, "dtTra").Value, (int)UiTheme.Find<NumericUpDown>(this, "numSoKhach").Value); lblKetQua.Text = "Tìm thấy " + grid.Rows.Count + " phòng phù hợp"; }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void btnLapPhieu_Click(object sender, EventArgs e)
        {
            var grid = UiTheme.Find<DataGridView>(this, "dgvPhongTrong"); if (grid.CurrentRow == null) { MessageBox.Show("Hãy chọn phòng trống."); return; }
            var result = service.TaoDatPhong(Convert.ToString(UiTheme.Find<ComboBox>(this, "cboKhach").SelectedValue), Convert.ToString(grid.CurrentRow.Cells["Số phòng"].Value), UiTheme.Find<DateTimePicker>(this, "dtNhan").Value, UiTheme.Find<DateTimePicker>(this, "dtTra").Value, (int)UiTheme.Find<NumericUpDown>(this, "numSoKhach").Value, UiTheme.Find<NumericUpDown>(this, "numTienCoc").Value, UiTheme.Find<ComboBox>(this, "cboKenh").Text);
            MessageBox.Show(result.ThongBao); if (result.ThanhCong) { btnTimPhong_Click(null, EventArgs.Empty); TaiPhieuDat(); }
        }
        private void ThemNguoi_Click(object sender, EventArgs e) { var grid = UiTheme.Find<DataGridView>(this, "dgvPhieuDat"); if (grid.CurrentRow == null) return; var r = service.ThemNguoiO(Convert.ToString(grid.CurrentRow.Cells["Số phiếu"].Value), Convert.ToString(grid.CurrentRow.Cells["Phòng"].Value), UiTheme.Find<TextBox>(this, "txtNguoiO").Text, UiTheme.Find<TextBox>(this, "txtCCCD").Text, UiTheme.Find<TextBox>(this, "txtQuocTich").Text); MessageBox.Show(r.ThongBao); }
        private void NhanPhong_Click(object sender, EventArgs e) { var grid = UiTheme.Find<DataGridView>(this, "dgvPhieuDat"); if (grid.CurrentRow == null) return; var r = service.NhanPhong(Convert.ToString(grid.CurrentRow.Cells["Số phiếu"].Value), Convert.ToString(grid.CurrentRow.Cells["Phòng"].Value)); MessageBox.Show(r.ThongBao); if (r.ThanhCong) TaiPhieuDat(); }
        private void KhoiTaoKhachHang()
        {
            var page = tabs.TabPages[2]; var grid = UiTheme.Find<DataGridView>(page, "dgvData"); Action load = () => { grid.Columns.Clear(); grid.AutoGenerateColumns = true; grid.DataSource = service.LayKhachHang(UiTheme.Find<TextBox>(page, "txtTim").Text); };
            UiTheme.Find<TextBox>(page, "txtTim").TextChanged += (s, e) => load(); UiTheme.Find<Button>(page, "btnTaiLai").Click += (s, e) => load(); UiTheme.Find<Button>(page, "btnThem").Click += (s, e) => LuuKhach(page, false, load); UiTheme.Find<Button>(page, "btnSua").Click += (s, e) => LuuKhach(page, true, load); UiTheme.Find<Button>(page, "btnXoa").Click += (s, e) => XoaKhach(page, load); UiTheme.Find<Button>(page, "btnLamMoi").Click += (s, e) => { foreach (var box in UiTheme.FindAll<TextBox>(page).Where(x => x.Name.StartsWith("txtField"))) box.Clear(); };
            grid.SelectionChanged += (s, e) => { if (grid.CurrentRow == null || grid.CurrentRow.DataBoundItem == null) return; var f = UiTheme.FindAll<TextBox>(page).Where(x => x.Name.StartsWith("txtField")).OrderBy(x => x.Name).ToArray(); for (int i = 0; i < f.Length; i++) f[i].Text = Convert.ToString(grid.CurrentRow.Cells[i].Value); }; load();
        }
        private void LuuKhach(TabPage page, bool sua, Action reload) { var v = UiTheme.FindAll<TextBox>(page).Where(x => x.Name.StartsWith("txtField")).OrderBy(x => x.Name).Select(x => x.Text).ToArray(); var r = service.LuuKhach(sua, v[0], v[1], v[2], v[3], v[4]); MessageBox.Show(r.ThongBao); if (r.ThanhCong) { reload(); TaiKhachCombo(); } }
        private void XoaKhach(TabPage page, Action reload) { var ma = UiTheme.Find<TextBox>(page, "txtField0").Text; if (string.IsNullOrWhiteSpace(ma)) { MessageBox.Show("Hãy chọn khách hàng cần xóa."); return; } if (MessageBox.Show("Xóa khách hàng đang chọn?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return; var r = service.XoaKhach(ma); MessageBox.Show(r.ThongBao); if (r.ThanhCong) { reload(); TaiKhachCombo(); } }
    }
}

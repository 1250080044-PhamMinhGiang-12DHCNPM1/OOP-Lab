using System;
using System.Linq;
using System.Windows.Forms;
using QuanLyKhachSan.Services;

namespace QuanLyKhachSan.Forms
{
    public partial class FrmPhongTienNghi : Form
    {
        private readonly PhongTienNghiService service = new PhongTienNghiService();
        public FrmPhongTienNghi() { InitializeComponent(); Load += FrmPhongTienNghi_Load; }
        private void FrmPhongTienNghi_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 2; i++) { int tab = i; var page = tabs.TabPages[i]; UiTheme.Find<Button>(page, "btnTaiLai").Click += (s, a) => Tai(tab); UiTheme.Find<Button>(page, "btnLamMoi").Click += (s, a) => XoaNhap(page); UiTheme.Find<Button>(page, "btnThem").Click += (s, a) => Luu(tab, false); UiTheme.Find<Button>(page, "btnSua").Click += (s, a) => Luu(tab, true); UiTheme.Find<Button>(page, "btnXoa").Click += (s, a) => Xoa(tab); UiTheme.Find<TextBox>(page, "txtTim").TextChanged += (s, a) => Tai(tab); UiTheme.Find<DataGridView>(page, "dgvData").SelectionChanged += (s, a) => GanDuLieu(tab); Tai(tab); }
            UiTheme.Find<Button>(tabs.TabPages[2], "btnLapPhieu").Click += LapPhieu_Click; UiTheme.Find<Button>(tabs.TabPages[2], "btnTaiLapDat").Click += (s, a) => TaiLapDat(); TaiLapDat();
        }
        private void Tai(int tab) { var page = tabs.TabPages[tab]; var grid = UiTheme.Find<DataGridView>(page, "dgvData"); grid.Columns.Clear(); grid.AutoGenerateColumns = true; grid.DataSource = tab == 0 ? service.LayPhong(UiTheme.Find<TextBox>(page, "txtTim").Text) : service.LayTienNghi(UiTheme.Find<TextBox>(page, "txtTim").Text); }
        private string[] GiaTri(TabPage page) { return UiTheme.FindAll<TextBox>(page).Where(x => x.Name.StartsWith("txtField")).OrderBy(x => x.Name).Select(x => x.Text.Trim()).ToArray(); }
        private void Luu(int tab, bool sua)
        {
            string[] v = GiaTri(tabs.TabPages[tab]); QuanLyKhachSan.Models.KetQua result;
            if (tab == 0) { int suc; decimal gia; if (v.Length < 5 || !int.TryParse(v[2], out suc) || !decimal.TryParse(v[3], out gia)) { MessageBox.Show("Sức chứa và đơn giá phải là số."); return; } result = service.LuuPhong(sua, v[0], v[1], suc, gia, v[4]); }
            else { int stt; if (v.Length < 4 || !int.TryParse(v[2], out stt)) { MessageBox.Show("Số thứ tự phải là số nguyên."); return; } result = service.LuuTienNghi(sua, v[0], v[1], stt, v[3]); }
            MessageBox.Show(result.ThongBao); if (result.ThanhCong) Tai(tab);
        }
        private void Xoa(int tab) { string[] v = GiaTri(tabs.TabPages[tab]); if (v.Length == 0 || string.IsNullOrWhiteSpace(v[0])) { MessageBox.Show("Hãy chọn dữ liệu cần xóa."); return; } if (MessageBox.Show("Xóa dữ liệu đang chọn?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return; var r = tab == 0 ? service.Xoa("Phong", "SoPhong", v[0]) : service.Xoa("TienNghi", "MaTienNghi", v[0]); MessageBox.Show(r.ThongBao); if (r.ThanhCong) Tai(tab); }
        private void GanDuLieu(int tab) { var page = tabs.TabPages[tab]; var grid = UiTheme.Find<DataGridView>(page, "dgvData"); if (grid.CurrentRow == null || grid.CurrentRow.DataBoundItem == null) return; var fields = UiTheme.FindAll<TextBox>(page).Where(x => x.Name.StartsWith("txtField")).OrderBy(x => x.Name).ToArray(); int[] map = tab == 0 ? new[] { 0, 1, 3, 4, 5 } : new[] { 0, 1, 3, 4 }; for (int i = 0; i < fields.Length; i++) fields[i].Text = Convert.ToString(grid.CurrentRow.Cells[map[i]].Value); }
        private void XoaNhap(TabPage page) { foreach (var box in UiTheme.FindAll<TextBox>(page).Where(x => x.Name.StartsWith("txtField"))) box.Clear(); }
        private void TaiLapDat()
        {
            var page = tabs.TabPages[2]; var grid = UiTheme.Find<DataGridView>(page, "dgvLapDat"); grid.Columns.Clear(); grid.AutoGenerateColumns = true; grid.DataSource = service.LayLapDat();
            var tn = UiTheme.Find<ComboBox>(page, "cboTienNghi"); tn.DataSource = service.LayTienNghi(""); tn.DisplayMember = "Mã tiện nghi"; tn.ValueMember = "Mã tiện nghi";
            var p = UiTheme.Find<ComboBox>(page, "cboPhong"); p.DataSource = service.LayPhong(""); p.DisplayMember = "Số phòng"; p.ValueMember = "Số phòng";
        }
        private void LapPhieu_Click(object sender, EventArgs e) { var page = tabs.TabPages[2]; var r = service.LapDat(UiTheme.Find<TextBox>(page, "txtSoPhieu").Text, Convert.ToString(UiTheme.Find<ComboBox>(page, "cboTienNghi").SelectedValue), Convert.ToString(UiTheme.Find<ComboBox>(page, "cboPhong").SelectedValue), UiTheme.Find<DateTimePicker>(page, "dtNgayLap").Value, UiTheme.Find<ComboBox>(page, "cboTinhTrang").Text); MessageBox.Show(r.ThongBao); if (r.ThanhCong) TaiLapDat(); }
    }
}

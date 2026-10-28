using System;
using System.Linq;
using System.Windows.Forms;
using QuanLyKhachSan.Services;

namespace QuanLyKhachSan.Forms
{
    public partial class FrmDanhMuc : Form
    {
        private readonly DanhMucService service = new DanhMucService();
        public FrmDanhMuc() { InitializeComponent(); Load += FrmDanhMuc_Load; }
        private void FrmDanhMuc_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < tabs.TabPages.Count; i++)
            {
                int tabIndex = i; TabPage page = tabs.TabPages[i];
                var search = UiTheme.Find<TextBox>(page, "txtTim"); var grid = UiTheme.Find<DataGridView>(page, "dgvData");
                UiTheme.Find<Button>(page, "btnTaiLai").Click += (s, a) => Tai(tabIndex);
                UiTheme.Find<Button>(page, "btnLamMoi").Click += (s, a) => XoaNhap(page);
                UiTheme.Find<Button>(page, "btnThem").Click += (s, a) => Luu(tabIndex, false);
                UiTheme.Find<Button>(page, "btnSua").Click += (s, a) => Luu(tabIndex, true);
                UiTheme.Find<Button>(page, "btnXoa").Click += (s, a) => Xoa(tabIndex);
                search.TextChanged += (s, a) => Tai(tabIndex); grid.SelectionChanged += (s, a) => GanDuLieu(tabIndex);
                Tai(tabIndex);
            }
        }
        private void Tai(int index)
        {
            var page = tabs.TabPages[index]; var grid = UiTheme.Find<DataGridView>(page, "dgvData"); var search = UiTheme.Find<TextBox>(page, "txtTim");
            try { grid.Columns.Clear(); grid.AutoGenerateColumns = true; grid.DataSource = service.LayDanhSach(index, search.Text); }
            catch (Exception ex) { MessageBox.Show("Không tải được dữ liệu. " + ex.Message); }
        }
        private string[] GiaTri(TabPage page) { return UiTheme.FindAll<TextBox>(page).Where(x => x.Name.StartsWith("txtField")).OrderBy(x => x.Name).Select(x => x.Text.Trim()).ToArray(); }
        private void Luu(int index, bool sua) { var result = service.Luu(index, sua, GiaTri(tabs.TabPages[index])); MessageBox.Show(result.ThongBao); if (result.ThanhCong) { Tai(index); XoaNhap(tabs.TabPages[index]); } }
        private void Xoa(int index) { var values = GiaTri(tabs.TabPages[index]); if (values.Length == 0 || string.IsNullOrWhiteSpace(values[0])) { MessageBox.Show("Hãy chọn dữ liệu cần xóa."); return; } if (MessageBox.Show("Xóa dữ liệu đang chọn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return; var result = service.Xoa(index, values[0]); MessageBox.Show(result.ThongBao); if (result.ThanhCong) Tai(index); }
        private void XoaNhap(TabPage page) { foreach (TextBox box in UiTheme.FindAll<TextBox>(page).Where(x => x.Name.StartsWith("txtField"))) box.Clear(); }
        private void GanDuLieu(int index)
        {
            var page = tabs.TabPages[index]; var grid = UiTheme.Find<DataGridView>(page, "dgvData"); if (grid.CurrentRow == null || grid.CurrentRow.DataBoundItem == null) return;
            var fields = UiTheme.FindAll<TextBox>(page).Where(x => x.Name.StartsWith("txtField")).OrderBy(x => x.Name).ToArray(); int[] map = index == 4 ? new[] { 0, 1, 3, 4 } : Enumerable.Range(0, fields.Length).ToArray();
            for (int i = 0; i < fields.Length && i < map.Length; i++) fields[i].Text = Convert.ToString(grid.CurrentRow.Cells[map[i]].Value);
        }
    }
}

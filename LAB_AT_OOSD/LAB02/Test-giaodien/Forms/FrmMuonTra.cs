using System.Windows.Forms;

namespace Test_giaodien.Forms
{
    public class FrmMuonTra : ModuleForm
    {
        private readonly DataGridView gridPhieuMuon;
        private readonly DataGridView gridChiTiet;
        private readonly DataGridView gridPhieuPhat;

        public FrmMuonTra() : base("Mượn - trả sách", "Lập phiếu mượn, nhận trả và xử lý phạt")
        {
            TabControl tabs = new TabControl { Dock = DockStyle.Fill };
            gridPhieuMuon = CreateReadOnlyGrid();
            gridChiTiet = CreateReadOnlyGrid();
            gridPhieuPhat = CreateReadOnlyGrid();

            AddTab(tabs, "Phiếu mượn", gridPhieuMuon);
            AddTab(tabs, "Chi tiết mượn", gridChiTiet);
            AddTab(tabs, "Phiếu phạt", gridPhieuPhat);
            ContentPanel.Controls.Add(tabs);

            Shown += (sender, e) =>
            {
                LoadGrid(gridPhieuMuon,
                    @"SELECT pm.MaPhieuMuon AS [Mã phiếu], CONCAT(dg.Ho, N' ', dg.Ten) AS [Độc giả],
                             CONCAT(nv.Ho, N' ', nv.Ten) AS [Nhân viên], pm.NgayMuon AS [Ngày mượn], pm.NgayHenTra AS [Hẹn trả]
                      FROM PhieuMuon pm JOIN DocGia dg ON dg.MaDocGia = pm.MaDocGia
                      JOIN NhanVien nv ON nv.MaNhanVien = pm.MaNhanVien ORDER BY pm.MaPhieuMuon");
                LoadGrid(gridChiTiet,
                    @"SELECT ct.MaChiTiet AS [Mã chi tiết], ct.MaPhieuMuon AS [Mã phiếu], ds.TenSach AS [Tên sách],
                             ct.NgayTraThucTe AS [Ngày trả], ct.TinhTrangTra AS [Tình trạng]
                      FROM ChiTietPhieuMuon ct JOIN DauSach ds ON ds.MaDauSach = ct.MaDauSach ORDER BY ct.MaChiTiet");
                LoadGrid(gridPhieuPhat,
                    @"SELECT pp.MaPhieuPhat AS [Mã phạt], pp.MaChiTiet AS [Mã chi tiết], pp.NgayPhat AS [Ngày phạt],
                             pp.LyDo AS [Lý do], pp.PhiPhat AS [Phí phạt]
                      FROM PhieuPhat pp ORDER BY pp.MaPhieuPhat");
            };
        }

        private static void AddTab(TabControl tabs, string title, DataGridView grid)
        {
            TabPage tab = new TabPage(title);
            tab.Controls.Add(grid);
            tabs.TabPages.Add(tab);
        }
    }
}

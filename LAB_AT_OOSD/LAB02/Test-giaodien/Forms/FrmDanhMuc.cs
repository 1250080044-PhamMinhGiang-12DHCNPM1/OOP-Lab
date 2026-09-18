using System.Windows.Forms;

namespace Test_giaodien.Forms
{
    public class FrmDanhMuc : ModuleForm
    {
        private readonly DataGridView gridNhanVien;
        private readonly DataGridView gridTheLoai;
        private readonly DataGridView gridNhaXuatBan;

        public FrmDanhMuc() : base("Danh mục", "Nhân viên - Thể loại - Nhà xuất bản")
        {
            TabControl tabs = new TabControl { Dock = DockStyle.Fill };
            gridNhanVien = CreateReadOnlyGrid();
            gridTheLoai = CreateReadOnlyGrid();
            gridNhaXuatBan = CreateReadOnlyGrid();

            AddTab(tabs, "Nhân viên", gridNhanVien);
            AddTab(tabs, "Thể loại", gridTheLoai);
            AddTab(tabs, "Nhà xuất bản", gridNhaXuatBan);
            ContentPanel.Controls.Add(tabs);

            Shown += (sender, e) =>
            {
                LoadGrid(gridNhanVien,
                    "SELECT MaNhanVien AS [Mã NV], CONCAT(Ho, N' ', Ten) AS [Họ tên], Phai AS [Phái], ChucVu AS [Chức vụ], SoDienThoai AS [SĐT] FROM NhanVien ORDER BY MaNhanVien");
                LoadGrid(gridTheLoai,
                    "SELECT MaTheLoai AS [Mã thể loại], TenTheLoai AS [Tên thể loại] FROM TheLoai ORDER BY MaTheLoai");
                LoadGrid(gridNhaXuatBan,
                    "SELECT MaNhaXuatBan AS [Mã NXB], DiaChi AS [Địa chỉ], SoDienThoai AS [SĐT] FROM NhaXuatBan ORDER BY MaNhaXuatBan");
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

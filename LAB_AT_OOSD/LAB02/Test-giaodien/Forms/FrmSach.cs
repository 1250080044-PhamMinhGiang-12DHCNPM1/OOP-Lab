namespace Test_giaodien.Forms
{
    public class FrmSach : ModuleForm
    {
        private readonly System.Windows.Forms.DataGridView gridSach;

        public FrmSach() : base("Quản lý đầu sách", "Thông tin đầu sách và số lượng trong kho")
        {
            gridSach = CreateReadOnlyGrid();
            ContentPanel.Controls.Add(gridSach);
            Shown += (sender, e) => LoadGrid(gridSach,
                @"SELECT s.MaDauSach AS [Mã sách], s.TenSach AS [Tên sách], s.NamXuatBan AS [Năm XB],
                         s.SoLuongHienCo AS [Số lượng], tl.TenTheLoai AS [Thể loại], nxb.MaNhaXuatBan AS [Mã NXB]
                  FROM DauSach s
                  JOIN TheLoai tl ON tl.MaTheLoai = s.MaTheLoai
                  JOIN NhaXuatBan nxb ON nxb.MaNhaXuatBan = s.MaNhaXuatBan
                  ORDER BY s.MaDauSach");
        }
    }
}

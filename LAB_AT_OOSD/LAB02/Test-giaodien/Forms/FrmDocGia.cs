namespace Test_giaodien.Forms
{
    public class FrmDocGia : ModuleForm
    {
        private readonly System.Windows.Forms.DataGridView gridDocGia;

        public FrmDocGia() : base("Độc giả và thẻ thư viện", "Quản lý thông tin độc giả, cấp và gia hạn thẻ")
        {
            gridDocGia = CreateReadOnlyGrid();
            ContentPanel.Controls.Add(gridDocGia);
            Shown += (sender, e) => LoadGrid(gridDocGia,
                @"SELECT dg.MaDocGia AS [Mã độc giả], CONCAT(dg.Ho, N' ', dg.Ten) AS [Họ tên],
                         dg.Email AS [Email], td.MaThe AS [Mã thẻ], td.HanSuDung AS [Hạn sử dụng],
                         CASE WHEN td.DaDongLePhi = 1 THEN N'Đã đóng' ELSE N'Chưa đóng' END AS [Lệ phí]
                  FROM DocGia dg
                  LEFT JOIN TheDocGia td ON td.MaDocGia = dg.MaDocGia AND td.TrangThai = 1
                  ORDER BY dg.MaDocGia");
        }
    }
}

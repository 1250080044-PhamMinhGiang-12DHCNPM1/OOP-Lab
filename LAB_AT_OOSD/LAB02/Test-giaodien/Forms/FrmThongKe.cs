namespace Test_giaodien.Forms
{
    public class FrmThongKe : ModuleForm
    {
        private readonly System.Windows.Forms.DataGridView gridThongKe;

        public FrmThongKe() : base("Thống kê", "Tổng hợp dữ liệu mượn, trả và phí phạt")
        {
            gridThongKe = CreateReadOnlyGrid();
            ContentPanel.Controls.Add(gridThongKe);
            Shown += (sender, e) => LoadGrid(gridThongKe,
                @"SELECT N'Đầu sách' AS [Chỉ số], COUNT(*) AS [Giá trị] FROM DauSach
                  UNION ALL SELECT N'Độc giả', COUNT(*) FROM DocGia
                  UNION ALL SELECT N'Phiếu mượn', COUNT(*) FROM PhieuMuon
                  UNION ALL SELECT N'Sách chưa trả', COUNT(*) FROM ChiTietPhieuMuon WHERE NgayTraThucTe IS NULL
                  UNION ALL SELECT N'Tổng phí phạt', ISNULL(SUM(PhiPhat), 0) FROM PhieuPhat");
        }
    }
}

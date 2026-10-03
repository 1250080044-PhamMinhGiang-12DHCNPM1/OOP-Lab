using System.Collections.Generic;
using WindowsFormsApp1.Data;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Adapters
{
    // Là điểm thay thế khi hệ thống quản lý sản phẩm dùng nguồn dữ liệu khác.
    public class BoDieuHopSanPham
    {
        private readonly KhoSanPham _khoSanPham = new KhoSanPham();

        public List<SanPham> LayDanhSach()
        {
            return _khoSanPham.LaySanPhamDangBan();
        }
    }
}

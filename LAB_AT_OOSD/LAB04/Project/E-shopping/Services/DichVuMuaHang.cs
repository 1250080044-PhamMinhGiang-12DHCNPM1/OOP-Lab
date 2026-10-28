using System;
using System.Collections.Generic;
using System.Linq;
using WindowsFormsApp1.Adapters;
using WindowsFormsApp1.Data;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Services
{
    public class DichVuMuaHang
    {
        private readonly BoDieuHopSanPham _sanPham = new BoDieuHopSanPham();
        private readonly KhoDonHang _khoDonHang = new KhoDonHang();
        private readonly BoDieuHopThanhToan _thanhToan = new BoDieuHopThanhToan();
        private readonly BoDieuHopThuDienTu _thuDienTu = new BoDieuHopThuDienTu();

        public List<SanPham> LaySanPham()
        {
            return _sanPham.LayDanhSach();
        }

        public int DatHang(int maKhachHang, List<MucGioHang> gioHang, NguoiNhan nguoiNhan, string soThe)
        {
            if (gioHang == null || gioHang.Count == 0) throw new InvalidOperationException("Giỏ hàng đang trống.");
            if (string.IsNullOrWhiteSpace(nguoiNhan.HoTen) || string.IsNullOrWhiteSpace(nguoiNhan.DiaChi))
                throw new InvalidOperationException("Cần nhập họ tên và địa chỉ người nhận.");

            decimal tongTien = gioHang.Sum(x => x.ThanhTien);
            if (!_thanhToan.ThanhToan(soThe, tongTien))
                throw new InvalidOperationException("Thanh toán không thành công. Số thẻ cần có ít nhất 6 chữ số.");

            DonHang donHang = new DonHang
            {
                MaKhachHang = maKhachHang,
                NguoiNhan = nguoiNhan,
                ChiTiet = gioHang,
                TongTien = tongTien,
                TrangThai = "Đã thanh toán",
                NgayDat = DateTime.Now
            };
            int maDonHang = _khoDonHang.Luu(donHang, "Thành công");
            _thuDienTu.GuiXacNhan(nguoiNhan.ThuDienTu, maDonHang);
            return maDonHang;
        }
    }
}

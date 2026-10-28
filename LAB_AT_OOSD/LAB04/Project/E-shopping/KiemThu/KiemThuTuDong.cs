using System;
using System.Collections.Generic;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Services;

namespace KiemThuTuDong
{
    internal class ChuongTrinhKiemThu
    {
        private static void Main()
        {
            DichVuMuaHang dichVu = new DichVuMuaHang();
            NguoiNhan nguoiNhan = new NguoiNhan
            {
                HoTen = "Người kiểm thử",
                SoDienThoai = "0900000000",
                DiaChi = "Địa chỉ kiểm thử",
                HinhThucGiaoHang = "Giao hàng thường",
                ThuDienTu = "test@example.com"
            };

            KiemThuGioTrong(dichVu, nguoiNhan);
            List<MucGioHang> gioHang = TaoGioHang(dichVu);
            KiemThuThanhToanThatBai(dichVu, gioHang, nguoiNhan);
            KiemThuDatHangThanhCong(dichVu, gioHang, nguoiNhan);
        }

        private static List<MucGioHang> TaoGioHang(DichVuMuaHang dichVu)
        {
            SanPham sanPham = dichVu.LaySanPham()[0];
            return new List<MucGioHang> { new MucGioHang { SanPham = sanPham, SoLuong = 1 } };
        }

        private static void KiemThuGioTrong(DichVuMuaHang dichVu, NguoiNhan nguoiNhan)
        {
            try
            {
                dichVu.DatHang(1, new List<MucGioHang>(), nguoiNhan, "123456");
                Console.WriteLine("TC04: KHÔNG ĐẠT");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("TC04: ĐẠT");
            }
        }

        private static void KiemThuThanhToanThatBai(DichVuMuaHang dichVu, List<MucGioHang> gioHang, NguoiNhan nguoiNhan)
        {
            try
            {
                dichVu.DatHang(1, gioHang, nguoiNhan, "123");
                Console.WriteLine("TC05: KHÔNG ĐẠT");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("TC05: ĐẠT");
            }
        }

        private static void KiemThuDatHangThanhCong(DichVuMuaHang dichVu, List<MucGioHang> gioHang, NguoiNhan nguoiNhan)
        {
            int maDonHang = dichVu.DatHang(1, gioHang, nguoiNhan, "123456");
            Console.WriteLine("TC06: ĐẠT - Mã đơn hàng " + maDonHang);
        }
    }
}

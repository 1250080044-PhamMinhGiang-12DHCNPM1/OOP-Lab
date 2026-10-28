using System;
using System.Collections.Generic;

namespace WindowsFormsApp1.Models
{
    public class DonHang
    {
        public int MaDonHang { get; set; }
        public int MaKhachHang { get; set; }
        public NguoiNhan NguoiNhan { get; set; }
        public List<MucGioHang> ChiTiet { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }
        public DateTime NgayDat { get; set; }
    }
}

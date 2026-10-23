using System;

namespace QuanLyNhanXu.Models
{
    public sealed class ThuViecInput
    {
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string CCCD { get; set; }
        public string DienThoai { get; set; }
        public string DiaChi { get; set; }
        public int MaPhongBan { get; set; }
        public int? MaNhuCau { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime DenNgayThuViec { get; set; }
        public decimal LuongThuViec { get; set; }
        public string DieuKhoan { get; set; }
    }

    public sealed class HopDongInput
    {
        public string MaNV { get; set; }
        public string LoaiHopDong { get; set; }
        public DateTime NgayKy { get; set; }
        public DateTime TuNgay { get; set; }
        public DateTime? DenNgay { get; set; }
    }

    public sealed class QuyetDinhTaiChinhInput
    {
        public string DoiTuong { get; set; }
        public string MaNV { get; set; }
        public int? MaPhongBan { get; set; }
        public DateTime Ngay { get; set; }
        public string LyDo { get; set; }
        public decimal SoTien { get; set; }
        public string HinhThuc { get; set; }
        public string NguoiKy { get; set; }
    }
}

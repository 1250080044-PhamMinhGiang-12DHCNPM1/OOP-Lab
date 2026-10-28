using System;
using System.Data;
using QuanLyKhachSan.Data;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.Services
{
    public class DanhMucService
    {
        public DataTable LayDanhSach(int tab, string keyword)
        {
            string k = "%" + (keyword ?? "").Trim() + "%";
            switch (tab)
            {
                case 0: return Db.Query("SELECT MaKhuVuc [Mã khu vực],TenKhuVuc [Tên khu vực] FROM KhuVuc WHERE MaKhuVuc LIKE @k OR TenKhuVuc LIKE @k ORDER BY MaKhuVuc", Db.P("@k", k));
                case 1: return Db.Query("SELECT MaNV [Mã NV],HoTen [Họ tên],VaiTro [Vai trò],SoDienThoai [Số điện thoại] FROM NhanVien WHERE MaNV LIKE @k OR HoTen LIKE @k ORDER BY MaNV", Db.P("@k", k));
                case 2: return Db.Query("SELECT MaLoaiTN [Mã loại],TenLoaiTN [Tên loại] FROM LoaiTienNghi WHERE MaLoaiTN LIKE @k OR TenLoaiTN LIKE @k ORDER BY MaLoaiTN", Db.P("@k", k));
                case 3: return Db.Query("SELECT MaDV [Mã DV],TenDV [Tên dịch vụ],DonViTinh [Đơn vị],DonGia [Đơn giá] FROM DichVu WHERE MaDV LIKE @k OR TenDV LIKE @k ORDER BY MaDV", Db.P("@k", k));
                default: return Db.Query(@"SELECT q.MaQuyDinh [Mã QĐ],q.MaLoaiTN [Mã loại],l.TenLoaiTN [Loại tiện nghi],q.MucDoThietHai [Mức độ],q.MucDenBu [Mức đền bù]
FROM QuyDinhDenBu q JOIN LoaiTienNghi l ON l.MaLoaiTN=q.MaLoaiTN WHERE q.MaQuyDinh LIKE @k OR l.TenLoaiTN LIKE @k ORDER BY q.MaQuyDinh", Db.P("@k", k));
            }
        }

        public KetQua Luu(int tab, bool sua, string[] v)
        {
            try
            {
                if (v == null || v.Length == 0 || string.IsNullOrWhiteSpace(v[0])) return KetQua.Loi("Mã không được để trống.");
                switch (tab)
                {
                    case 0:
                        if (v.Length < 2 || string.IsNullOrWhiteSpace(v[1])) return KetQua.Loi("Tên khu vực không được để trống.");
                        if (sua) Db.Execute("UPDATE KhuVuc SET TenKhuVuc=@ten WHERE MaKhuVuc=@ma", Db.P("@ten", v[1]), Db.P("@ma", v[0]));
                        else Db.Execute("INSERT KhuVuc(MaKhuVuc,TenKhuVuc) VALUES(@ma,@ten)", Db.P("@ma", v[0]), Db.P("@ten", v[1])); break;
                    case 1:
                        if (v.Length < 4 || string.IsNullOrWhiteSpace(v[1]) || string.IsNullOrWhiteSpace(v[2])) return KetQua.Loi("Họ tên và vai trò không được để trống.");
                        if (sua) Db.Execute("UPDATE NhanVien SET HoTen=@ten,VaiTro=@vt,SoDienThoai=@sdt WHERE MaNV=@ma", Db.P("@ten", v[1]), Db.P("@vt", v[2]), Db.P("@sdt", v[3]), Db.P("@ma", v[0]));
                        else Db.Execute("INSERT NhanVien(MaNV,HoTen,VaiTro,SoDienThoai) VALUES(@ma,@ten,@vt,@sdt)", Db.P("@ma", v[0]), Db.P("@ten", v[1]), Db.P("@vt", v[2]), Db.P("@sdt", v[3])); break;
                    case 2:
                        if (v.Length < 2 || string.IsNullOrWhiteSpace(v[1])) return KetQua.Loi("Tên loại tiện nghi không được để trống.");
                        if (sua) Db.Execute("UPDATE LoaiTienNghi SET TenLoaiTN=@ten WHERE MaLoaiTN=@ma", Db.P("@ten", v[1]), Db.P("@ma", v[0]));
                        else Db.Execute("INSERT LoaiTienNghi(MaLoaiTN,TenLoaiTN) VALUES(@ma,@ten)", Db.P("@ma", v[0]), Db.P("@ten", v[1])); break;
                    case 3:
                        decimal gia; if (v.Length < 4 || !decimal.TryParse(v[3], out gia) || gia < 0) return KetQua.Loi("Đơn giá phải là số không âm.");
                        if (sua) Db.Execute("UPDATE DichVu SET TenDV=@ten,DonViTinh=@dvt,DonGia=@gia WHERE MaDV=@ma", Db.P("@ten", v[1]), Db.P("@dvt", v[2]), Db.P("@gia", gia), Db.P("@ma", v[0]));
                        else Db.Execute("INSERT DichVu(MaDV,TenDV,DonViTinh,DonGia) VALUES(@ma,@ten,@dvt,@gia)", Db.P("@ma", v[0]), Db.P("@ten", v[1]), Db.P("@dvt", v[2]), Db.P("@gia", gia)); break;
                    default:
                        decimal denBu; if (v.Length < 4 || !decimal.TryParse(v[3], out denBu) || denBu < 0) return KetQua.Loi("Mức đền bù phải là số không âm.");
                        if (sua) Db.Execute("UPDATE QuyDinhDenBu SET MaLoaiTN=@loai,MucDoThietHai=@muc,MucDenBu=@tien WHERE MaQuyDinh=@ma", Db.P("@loai", v[1]), Db.P("@muc", v[2]), Db.P("@tien", denBu), Db.P("@ma", v[0]));
                        else Db.Execute("INSERT QuyDinhDenBu(MaQuyDinh,MaLoaiTN,MucDoThietHai,MucDenBu) VALUES(@ma,@loai,@muc,@tien)", Db.P("@ma", v[0]), Db.P("@loai", v[1]), Db.P("@muc", v[2]), Db.P("@tien", denBu)); break;
                }
                return KetQua.Ok(sua ? "Cập nhật thành công." : "Thêm mới thành công.");
            }
            catch (Exception ex) { return KetQua.Loi("Không thể lưu dữ liệu. " + ex.Message); }
        }

        public KetQua Xoa(int tab, string ma)
        {
            try
            {
                string[] tables = { "KhuVuc", "NhanVien", "LoaiTienNghi", "DichVu", "QuyDinhDenBu" };
                string[] keys = { "MaKhuVuc", "MaNV", "MaLoaiTN", "MaDV", "MaQuyDinh" };
                int rows = Db.Execute("DELETE FROM " + tables[tab] + " WHERE " + keys[tab] + "=@ma", Db.P("@ma", ma));
                return rows == 0 ? KetQua.Loi("Không tìm thấy dữ liệu cần xóa.") : KetQua.Ok("Xóa thành công.");
            }
            catch (Exception) { return KetQua.Loi("Không thể xóa vì dữ liệu đã được sử dụng trong nghiệp vụ khác."); }
        }
    }
}

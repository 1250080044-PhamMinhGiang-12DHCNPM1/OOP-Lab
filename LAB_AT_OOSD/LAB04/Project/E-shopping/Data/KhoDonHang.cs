using System;
using System.Data.SqlClient;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Data
{
    public class KhoDonHang
    {
        public int Luu(DonHang donHang, string ketQuaThanhToan)
        {
            using (SqlConnection ketNoi = KetNoiDuLieu.TaoKetNoi())
            {
                ketNoi.Open();
                SqlTransaction giaoDich = ketNoi.BeginTransaction();
                try
                {
                    int maDonHang;
                    const string themDon = @"INSERT INTO DonHang(MaKhachHang, TenNguoiNhan, SoDienThoaiNguoiNhan, DiaChiGiaoHang, HinhThucGiaoHang, TongTien, TrangThai)
VALUES (@MaKhachHang, @TenNguoiNhan, @SoDienThoai, @DiaChi, @HinhThucGiaoHang, @TongTien, @TrangThai); SELECT CAST(SCOPE_IDENTITY() AS INT);";
                    using (SqlCommand lenh = new SqlCommand(themDon, ketNoi, giaoDich))
                    {
                        lenh.Parameters.AddWithValue("@MaKhachHang", donHang.MaKhachHang);
                        lenh.Parameters.AddWithValue("@TenNguoiNhan", donHang.NguoiNhan.HoTen);
                        lenh.Parameters.AddWithValue("@SoDienThoai", donHang.NguoiNhan.SoDienThoai);
                        lenh.Parameters.AddWithValue("@DiaChi", donHang.NguoiNhan.DiaChi);
                        lenh.Parameters.AddWithValue("@HinhThucGiaoHang", donHang.NguoiNhan.HinhThucGiaoHang);
                        lenh.Parameters.AddWithValue("@TongTien", donHang.TongTien);
                        lenh.Parameters.AddWithValue("@TrangThai", donHang.TrangThai);
                        maDonHang = (int)lenh.ExecuteScalar();
                    }

                    foreach (MucGioHang muc in donHang.ChiTiet)
                    {
                        using (SqlCommand lenh = new SqlCommand("INSERT INTO ChiTietDonHang(MaDonHang, MaSanPham, SoLuong, DonGia) VALUES (@MaDonHang, @MaSanPham, @SoLuong, @DonGia)", ketNoi, giaoDich))
                        {
                            lenh.Parameters.AddWithValue("@MaDonHang", maDonHang);
                            lenh.Parameters.AddWithValue("@MaSanPham", muc.SanPham.MaSanPham);
                            lenh.Parameters.AddWithValue("@SoLuong", muc.SoLuong);
                            lenh.Parameters.AddWithValue("@DonGia", muc.SanPham.DonGia);
                            lenh.ExecuteNonQuery();
                        }
                    }

                    using (SqlCommand lenh = new SqlCommand("INSERT INTO GiaoDichThanhToan(MaDonHang, SoTien, KetQua) VALUES (@MaDonHang, @SoTien, @KetQua)", ketNoi, giaoDich))
                    {
                        lenh.Parameters.AddWithValue("@MaDonHang", maDonHang);
                        lenh.Parameters.AddWithValue("@SoTien", donHang.TongTien);
                        lenh.Parameters.AddWithValue("@KetQua", ketQuaThanhToan);
                        lenh.ExecuteNonQuery();
                    }
                    giaoDich.Commit();
                    return maDonHang;
                }
                catch
                {
                    giaoDich.Rollback();
                    throw;
                }
            }
        }
    }
}

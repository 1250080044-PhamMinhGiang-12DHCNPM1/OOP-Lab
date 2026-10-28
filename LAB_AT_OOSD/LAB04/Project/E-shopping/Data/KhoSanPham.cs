using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1.Data
{
    public class KhoSanPham
    {
        public List<SanPham> LaySanPhamDangBan()
        {
            var ketQua = new List<SanPham>();
            const string cauLenh = "SELECT MaSanPham, MaHienThi, TenSanPham, DonGia, SoLuongTon FROM SanPham WHERE DangBan = 1";

            using (SqlConnection ketNoi = KetNoiDuLieu.TaoKetNoi())
            using (SqlCommand lenh = new SqlCommand(cauLenh, ketNoi))
            {
                ketNoi.Open();
                using (SqlDataReader doc = lenh.ExecuteReader())
                {
                    while (doc.Read())
                    {
                        ketQua.Add(new SanPham
                        {
                            MaSanPham = Convert.ToInt32(doc["MaSanPham"]),
                            MaHienThi = doc["MaHienThi"].ToString(),
                            TenSanPham = doc["TenSanPham"].ToString(),
                            DonGia = Convert.ToDecimal(doc["DonGia"]),
                            SoLuongTon = Convert.ToInt32(doc["SoLuongTon"])
                        });
                    }
                }
            }
            return ketQua;
        }
    }
}

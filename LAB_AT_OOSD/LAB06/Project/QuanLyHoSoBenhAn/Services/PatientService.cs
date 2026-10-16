using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyHoSoBenhAn.Data;
using QuanLyHoSoBenhAn.Models;

namespace QuanLyHoSoBenhAn.Services
{
    public sealed class PatientService
    {
        public string RegisterAndCreateRecord(PatientRegistration model)
        {
            if (string.IsNullOrWhiteSpace(model.FullName) || string.IsNullOrWhiteSpace(model.Phone))
                throw new ArgumentException("Họ tên và điện thoại là bắt buộc.");
            if (model.DateOfBirth.Date > DateTime.Today)
                throw new ArgumentException("Ngày sinh không hợp lệ.");
            if (model.AnnualFee <= 0)
                throw new ArgumentException("Lệ phí năm phải lớn hơn 0.");

            using (var connection = Db.OpenConnection())
            using (var command = new SqlCommand("dbo.sp_DangKyCapHoSo", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@HoTen", model.FullName.Trim());
                command.Parameters.AddWithValue("@NgaySinh", model.DateOfBirth.Date);
                command.Parameters.AddWithValue("@DiaChi", (object)(model.Address ?? string.Empty));
                command.Parameters.AddWithValue("@DienThoai", model.Phone.Trim());
                command.Parameters.AddWithValue("@Anh3x4", (object)(model.PhotoPath ?? string.Empty));
                command.Parameters.AddWithValue("@SoTienLePhi", model.AnnualFee);
                command.Parameters.AddWithValue("@Nam", model.Year);
                var output = new SqlParameter("@MaHoSo", SqlDbType.VarChar, 20) { Direction = ParameterDirection.Output };
                command.Parameters.Add(output);
                command.ExecuteNonQuery();
                return Convert.ToString(output.Value);
            }
        }

        public DataTable SearchHistory(string keyword)
        {
            const string sql = @"SELECT h.MaHoSo, b.HoTen, b.NgaySinh, b.DienThoai, p.MaPK, p.NgayGioKham,
                                        p.TrieuChung, p.ChanDoan, p.TrangThai, bs.HoTen AS BacSi, k.TenKhoa
                                 FROM HoSoBenhAn h INNER JOIN BenhNhan b ON h.MaBN=b.MaBN
                                 LEFT JOIN PhieuKham p ON p.MaHoSo=h.MaHoSo
                                 LEFT JOIN BacSi bs ON bs.MaBS=p.MaBS LEFT JOIN Khoa k ON k.MaKhoa=p.MaKhoa
                                 WHERE h.MaHoSo LIKE @q OR b.HoTen LIKE @q OR b.DienThoai LIKE @q
                                 ORDER BY p.NgayGioKham DESC";
            return Db.Query(sql, new SqlParameter("@q", "%" + (keyword ?? string.Empty).Trim() + "%"));
        }

        public void CreateQuarterlyAppointment(string recordCode, DateTime appointment)
        {
            if (string.IsNullOrWhiteSpace(recordCode)) throw new ArgumentException("Nhập mã hồ sơ.");
            if (appointment <= DateTime.Now) throw new ArgumentException("Lịch hẹn phải ở tương lai.");
            const string sql = @"IF NOT EXISTS (SELECT 1 FROM HoSoBenhAn WHERE MaHoSo=@MaHoSo AND TrangThai=N'Đang sử dụng')
                                     THROW 52001, N'Hồ sơ không hợp lệ.', 1;
                                 INSERT INTO LichHen(MaHoSo,NgayGioHen,LoaiHen) VALUES(@MaHoSo,@NgayGioHen,N'Định kỳ');";
            Db.Execute(sql, new SqlParameter("@MaHoSo", recordCode.Trim()), new SqlParameter("@NgayGioHen", appointment));
        }
    }
}

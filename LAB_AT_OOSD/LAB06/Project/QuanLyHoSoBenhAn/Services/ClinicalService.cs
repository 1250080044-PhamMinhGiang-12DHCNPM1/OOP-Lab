using System;
using System.Data;
using System.Data.SqlClient;
using QuanLyHoSoBenhAn.Data;

namespace QuanLyHoSoBenhAn.Services
{
    public sealed class ClinicalService
    {
        public int CreateExaminationAndPrescription(string recordCode, int doctorId, int departmentId, string symptoms, string diagnosis, string testRequest, string instruction, int medicineId, int quantity, string dosage)
        {
            if (string.IsNullOrWhiteSpace(recordCode) || quantity <= 0 || medicineId <= 0)
                throw new ArgumentException("Mã hồ sơ, thuốc và số lượng phải hợp lệ.");
            var details = new DataTable();
            details.Columns.Add("MaThuoc", typeof(int)); details.Columns.Add("SoLuong", typeof(int));
            details.Columns.Add("LieuDung", typeof(string)); details.Columns.Add("CachDung", typeof(string));
            details.Rows.Add(medicineId, quantity, dosage ?? string.Empty, instruction ?? string.Empty);
            using (var connection = Db.OpenConnection())
            using (var command = new SqlCommand("dbo.sp_LapPhieuKhamVaToa", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@MaHoSo", recordCode.Trim());
                command.Parameters.AddWithValue("@MaBS", doctorId); command.Parameters.AddWithValue("@MaKhoa", departmentId);
                command.Parameters.AddWithValue("@TrieuChung", symptoms ?? string.Empty); command.Parameters.AddWithValue("@ChanDoan", diagnosis ?? string.Empty);
                command.Parameters.AddWithValue("@NoiDungXetNghiem", string.IsNullOrWhiteSpace(testRequest) ? (object)DBNull.Value : testRequest);
                command.Parameters.AddWithValue("@LoiDan", instruction ?? string.Empty);
                var tvp = command.Parameters.AddWithValue("@ChiTiet", details); tvp.SqlDbType = SqlDbType.Structured; tvp.TypeName = "dbo.TT_ChiTietToa";
                var visit = new SqlParameter("@MaPK", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var prescription = new SqlParameter("@MaToa", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(visit); command.Parameters.Add(prescription); command.ExecuteNonQuery();
                return Convert.ToInt32(visit.Value);
            }
        }

        public void PayAndComplete(int visitId, decimal amount, string method)
        {
            if (visitId <= 0 || amount < 0) throw new ArgumentException("Mã phiếu khám hoặc số tiền không hợp lệ.");
            using (var connection = Db.OpenConnection())
            using (var command = new SqlCommand("dbo.sp_ThanhToanHoanTat", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@MaPK", visitId); command.Parameters.AddWithValue("@SoTien", amount); command.Parameters.AddWithValue("@PhuongThuc", method);
                command.Parameters.Add(new SqlParameter("@MaTT", SqlDbType.Int) { Direction = ParameterDirection.Output });
                command.ExecuteNonQuery();
            }
        }
    }
}

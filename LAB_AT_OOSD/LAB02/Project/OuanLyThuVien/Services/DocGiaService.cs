using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyThuVien.Data;

public KetQuaXuLy CapThe(string maDocGia, DateTime ngayCap, DateTime hanSuDung, bool daDongLePhi)
{
    if (hanSuDung.Date < ngayCap.Date) return KetQuaXuLy.Loi("Hạn sử dụng phải từ ngày cấp trở đi.");
    object tonTai = Db.Scalar("SELECT COUNT(*) FROM DocGia WHERE MaDocGia=@Ma", new SqlParameter("@Ma",
   maDocGia));
    if (Convert.ToInt32(tonTai) == 0) return KetQuaXuLy.Loi("Không tìm thấy độc giả.");
    using (SqlConnection cn = Db.OpenConnection())
    using (SqlTransaction tx = cn.BeginTransaction())
    {
        try
        {
            using (SqlCommand check = new SqlCommand(@"SELECT COUNT(*) FROM TheDocGia
 WHERE MaDocGia=@Ma AND TrangThai=1 AND HanSuDung>=@NgayCap", cn, tx))
            {
                check.Parameters.AddWithValue("@Ma", maDocGia);
                check.Parameters.AddWithValue("@NgayCap", ngayCap.Date);
                if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                    return KetQuaXuLy.Loi("Độc giả đang có một thẻ còn giá trị sử dụng.");
            }
            using (SqlCommand off = new SqlCommand("UPDATE TheDocGia SET TrangThai=0 WHERE MaDocGia=@Ma AND
           TrangThai = 1", cn, tx))
            {
 off.Parameters.AddWithValue("@Ma", maDocGia);
 off.ExecuteNonQuery();
 }
            string maThe = "THE_" + maDocGia + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            using (SqlCommand ins = new SqlCommand(@"INSERT INTO
TheDocGia(MaThe,MaDocGia,NgayCap,HanSuDung,DaDongLePhi,TrangThai)
 VALUES(@MaThe,@MaDG,@NgayCap,@Han, @LePhi,1)", cn, tx))
            {
                ins.Parameters.AddWithValue("@MaThe", maThe);
                ins.Parameters.AddWithValue("@MaDG", maDocGia);
                ins.Parameters.AddWithValue("@NgayCap", ngayCap.Date);
                ins.Parameters.AddWithValue("@Han", hanSuDung.Date);
                ins.Parameters.AddWithValue("@LePhi", daDongLePhi);
                ins.ExecuteNonQuery();
            }
            tx.Commit();
            return KetQuaXuLy.Ok("Cấp thẻ thư viện thành công. Mã thẻ: " + maThe);
        }
        catch (Exception ex)
        {
// ... mã nguồn đầy đủ nằm trong project đính kèm ...
using System.Windows.Forms;

namespace ChoThueVanPhongCaoOc.Forms
{
    internal sealed class FrmKhachHang : Form
    {
        internal FrmKhachHang()
        {
            Text = "Quản lý khách hàng"; Width = 900; Height = 520; StartPosition = FormStartPosition.CenterParent;
            Controls.Add(new CrudPanel("SELECT * FROM KhachHang ORDER BY MaKH", "INSERT KhachHang(MaKH,TenKH,DienThoai,DiaChi) VALUES(@MaKH,@TenKH,@DienThoai,@DiaChi)", new FieldSpec("Mã KH", "@MaKH"), new FieldSpec("Tên khách hàng", "@TenKH"), new FieldSpec("Điện thoại", "@DienThoai"), new FieldSpec("Địa chỉ", "@DiaChi")));
        }
    }
}

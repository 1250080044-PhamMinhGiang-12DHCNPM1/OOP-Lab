using System.Windows.Forms;

namespace ChoThueVanPhongCaoOc.Forms
{
    internal sealed class FrmNhanVien : Form
    {
        internal FrmNhanVien(){Text="Công ty và nhân viên làm việc";Width=1000;Height=560;StartPosition=FormStartPosition.CenterParent;var tabs=new TabControl{Dock=DockStyle.Fill};var ct=new TabPage("Công ty");ct.Controls.Add(new CrudPanel("SELECT * FROM CongTy ORDER BY MaCongTy","INSERT CongTy(MaCongTy,TenCongTy,DienThoai) VALUES(@MaCongTy,@TenCongTy,@DienThoai)",new FieldSpec("Mã công ty","@MaCongTy"),new FieldSpec("Tên công ty","@TenCongTy"),new FieldSpec("Điện thoại","@DienThoai")));var nv=new TabPage("Nhân viên");nv.Controls.Add(new CrudPanel("SELECT n.*,c.TenCongTy FROM NhanVien n JOIN CongTy c ON c.MaCongTy=n.MaCongTy ORDER BY MaNV","INSERT NhanVien(MaNV,MaCongTy,MaPhong,HoTen,ChucVu,HinhAnh) VALUES(@MaNV,@MaCongTy,@MaPhong,@HoTen,@ChucVu,@HinhAnh)",new FieldSpec("Mã NV","@MaNV"),new FieldSpec("Mã công ty","@MaCongTy"),new FieldSpec("Mã phòng","@MaPhong"),new FieldSpec("Họ tên","@HoTen"),new FieldSpec("Chức vụ","@ChucVu"),new FieldSpec("Hình ảnh","@HinhAnh")));tabs.TabPages.Add(ct);tabs.TabPages.Add(nv);Controls.Add(tabs);}
    }
}

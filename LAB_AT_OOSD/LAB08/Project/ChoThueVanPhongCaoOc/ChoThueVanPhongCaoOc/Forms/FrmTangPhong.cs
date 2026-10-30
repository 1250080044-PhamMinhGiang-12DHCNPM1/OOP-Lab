using System.Windows.Forms;

namespace ChoThueVanPhongCaoOc.Forms
{
    internal sealed class FrmTangPhong : Form
    {
        internal FrmTangPhong()
        {
            Text = "Quản lý tầng và phòng"; Width = 950; Height = 560; StartPosition = FormStartPosition.CenterParent;
            var tabs = new TabControl { Dock = DockStyle.Fill };
            var tang = new TabPage("Tầng"); tang.Controls.Add(new CrudPanel("SELECT * FROM Tang ORDER BY MaTang", "INSERT Tang(MaTang,TenTang) VALUES(@MaTang,@TenTang)", new FieldSpec("Mã tầng", "@MaTang"), new FieldSpec("Tên tầng", "@TenTang")));
            var phong = new TabPage("Phòng"); phong.Controls.Add(new CrudPanel("SELECT p.*,t.TenTang FROM Phong p JOIN Tang t ON t.MaTang=p.MaTang ORDER BY p.MaPhong", "INSERT Phong(MaPhong,MaTang,DienTich,Huong,GiaThue) VALUES(@MaPhong,@MaTang,@DienTich,@Huong,@GiaThue)", new FieldSpec("Mã phòng", "@MaPhong"), new FieldSpec("Mã tầng", "@MaTang"), new FieldSpec("Diện tích", "@DienTich", FieldKind.Decimal), new FieldSpec("Hướng", "@Huong"), new FieldSpec("Giá thuê", "@GiaThue", FieldKind.Decimal)));
            tabs.TabPages.Add(tang); tabs.TabPages.Add(phong); Controls.Add(tabs);
        }
    }
}

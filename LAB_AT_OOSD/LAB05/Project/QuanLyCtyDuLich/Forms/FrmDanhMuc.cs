using System;
using System.Windows.Forms;
using QuanLyCongTyDuLich.Services;

namespace QuanLyCongTyDuLich.Forms
{
    public class FrmDanhMuc : Form
    {
        private readonly DanhMucService svc=new DanhMucService(); private readonly DataGridView grid=Ui.Grid();
        public FrmDanhMuc(){ Text="Danh mục"; Width=900; Height=560; var top=Ui.Top(); top.Controls.Add(Ui.Button("Phương tiện",(s,e)=>LoadData(1)));top.Controls.Add(Ui.Button("Điểm bán vé",(s,e)=>LoadData(2)));top.Controls.Add(Ui.Button("Hướng dẫn viên",(s,e)=>LoadData(3)));top.Controls.Add(Ui.Button("Điểm tham quan",(s,e)=>LoadData(4))); Controls.Add(grid);Controls.Add(top);LoadData(1); }
        private void LoadData(int kind){ if(kind==1)grid.DataSource=svc.LayPhuongTien();else if(kind==2)grid.DataSource=svc.LayDiemBan();else if(kind==3)grid.DataSource=svc.LayHDV();else grid.DataSource=svc.LayDiemThamQuan(); }
    }
}

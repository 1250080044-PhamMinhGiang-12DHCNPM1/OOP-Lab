using System;
using System.Windows.Forms;
using QuanLyCongTyDuLich.Services;

namespace QuanLyCongTyDuLich.Forms
{
    public class FrmTour : Form
    {
        private readonly TourService svc=new TourService(); private readonly DataGridView grid=Ui.Grid(); private readonly TextBox ma=Ui.Text("Mã tour"),ten=Ui.Text("Tên tour",190),mota=Ui.Text("Mô tả",190); private readonly NumericUpDown ngay=Ui.Number(3,30),dem=Ui.Number(2,30),gia=Ui.Number(2500000,999999999);
        public FrmTour(){Text="Tour - hành trình";Width=1040;Height=600;var top=Ui.Top();top.Controls.AddRange(new Control[]{ma,ten,ngay,dem,gia,mota,Ui.Button("Thêm tour",Them),Ui.Button("Nạp lại",(s,e)=>LoadData())});Controls.Add(grid);Controls.Add(top);LoadData();}
        private void LoadData(){grid.DataSource=svc.LayTour();}
        private void Them(object s,EventArgs e){Ui.Show(svc.ThemTour(ma.Text,ten.Text,(int)ngay.Value,(int)dem.Value,gia.Value,mota.Text));LoadData();}
    }
}

using System;
using System.Windows.Forms;
using QuanLyCongTyDuLich.Services;

namespace QuanLyCongTyDuLich.Forms
{
    public class FrmChuyenLe : Form
    {
        private readonly ChuyenLeService svc=new ChuyenLeService();private readonly DataGridView grid=Ui.Grid();private readonly TextBox ma=Ui.Text("Mã chuyến"),tour=Ui.Text("Mã tour"),don=Ui.Text("Địa điểm đón",220);private readonly DateTimePicker ngay=new DateTimePicker { Value=DateTime.Today.AddDays(30),Width=135,Margin=new Padding(4)};
        public FrmChuyenLe(){Text="Lịch chuyến khách lẻ";Width=990;Height=560;var top=Ui.Top();top.Controls.AddRange(new Control[]{ma,tour,ngay,don,Ui.Button("Tạo chuyến",Them),Ui.Button("Đóng đăng ký chuyến chọn",Dong),Ui.Button("Nạp lại",(s,e)=>LoadData())});Controls.Add(grid);Controls.Add(top);LoadData();}
        private void LoadData(){grid.DataSource=svc.LayChuyen();}
        private void Them(object s,EventArgs e){Ui.Show(svc.ThemChuyen(ma.Text,tour.Text,ngay.Value,don.Text));LoadData();}
        private void Dong(object s,EventArgs e){Ui.Show(svc.DongDangKy(Ui.Cell(grid,"MaChuyen")));LoadData();}
    }
}

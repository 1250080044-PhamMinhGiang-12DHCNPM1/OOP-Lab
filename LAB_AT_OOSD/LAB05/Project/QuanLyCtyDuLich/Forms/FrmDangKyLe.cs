using System;
using System.Windows.Forms;
using QuanLyCongTyDuLich.Services;

namespace QuanLyCongTyDuLich.Forms
{
    public class FrmDangKyLe : Form
    {
        private readonly DangKyLeService svc=new DangKyLeService();private readonly DataGridView grid=Ui.Grid();private readonly TextBox so=Ui.Text("Số đăng ký"),chuyen=Ui.Text("Mã chuyến"),ban=Ui.Text("Mã điểm bán"),ten=Ui.Text("Người đăng ký"),dt=Ui.Text("Điện thoại");private readonly NumericUpDown nguoi=Ui.Number(1,11);
        public FrmDangKyLe(){Text="Đăng ký và thanh toán vé khách lẻ";Width=1100;Height=560;var top=Ui.Top();top.Controls.AddRange(new Control[]{so,chuyen,ban,ten,dt,nguoi,Ui.Button("Đăng ký và thanh toán",DangKy),Ui.Button("Nạp lại",(s,e)=>LoadData())});Controls.Add(grid);Controls.Add(top);LoadData();}
        private void LoadData(){grid.DataSource=svc.LayDanhSach();}
        private void DangKy(object s,EventArgs e){Ui.Show(svc.DangKy(so.Text,chuyen.Text,ban.Text,ten.Text,dt.Text,(int)nguoi.Value));LoadData();}
    }
}

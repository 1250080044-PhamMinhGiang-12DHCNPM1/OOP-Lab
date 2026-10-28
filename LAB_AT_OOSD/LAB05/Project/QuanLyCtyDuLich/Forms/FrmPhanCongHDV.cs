using System;
using System.Windows.Forms;
using QuanLyCongTyDuLich.Services;

namespace QuanLyCongTyDuLich.Forms
{
    public class FrmPhanCongHDV : Form
    {
        private readonly PhanCongService svc=new PhanCongService();private readonly DataGridView grid=Ui.Grid();private readonly TextBox ma=Ui.Text("Mã phân công"),hdv=Ui.Text("Mã HDV"),doiTuong=Ui.Text("Mã chuyến/đoàn");private readonly ComboBox loai=new ComboBox{Width=90,DropDownStyle=ComboBoxStyle.DropDownList,Margin=new Padding(4)};private readonly NumericUpDown thuLao=Ui.Number(1000000,999999999);
        public FrmPhanCongHDV(){Text="Phân công hướng dẫn viên";Width=960;Height=560;loai.Items.AddRange(new object[]{"LE","DOAN"});loai.SelectedIndex=0;var top=Ui.Top();top.Controls.AddRange(new Control[]{ma,hdv,loai,doiTuong,thuLao,Ui.Button("Phân công",Them),Ui.Button("Nạp lại",(s,e)=>LoadData())});Controls.Add(grid);Controls.Add(top);LoadData();}
        private void LoadData(){grid.DataSource=svc.LayDanhSach();}
        private void Them(object s,EventArgs e){Ui.Show(svc.PhanCong(ma.Text,hdv.Text,loai.Text,doiTuong.Text,thuLao.Value));LoadData();}
    }
}

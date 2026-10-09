using System;
using System.Windows.Forms;
using QuanLyCongTyDuLich.Services;

namespace QuanLyCongTyDuLich.Forms
{
    public class FrmKetThucKhaoSat : Form
    {
        private readonly KetThucService svc=new KetThucService(); private readonly DataGridView grid=Ui.Grid(); private readonly TextBox soTT=Ui.Text("Số thanh toán"),soDK=Ui.Text("Số phiếu đoàn"),ghiChu=Ui.Text("Ghi chú",180),maKS=Ui.Text("Mã khảo sát"),soKSDangKy=Ui.Text("Số đăng ký"),gopY=Ui.Text("Góp ý",180); private readonly NumericUpDown tien=Ui.Number(1000000,999999999),diem=Ui.Number(5,5); private readonly DateTimePicker ngay=new DateTimePicker{Value=DateTime.Today,Width=125,Margin=new Padding(4)};private readonly ComboBox loai=new ComboBox{Width=75,DropDownStyle=ComboBoxStyle.DropDownList,Margin=new Padding(4)};
        public FrmKetThucKhaoSat(){Text="Kết thúc tour - thanh toán và khảo sát";Width=1120;Height=650;loai.Items.AddRange(new object[]{"LE","DOAN"});loai.SelectedIndex=0;var tabs=new TabControl{Dock=DockStyle.Fill};var pay=new TabPage("Thanh toán đoàn");var survey=new TabPage("Khảo sát khách hàng");var p1=Ui.Top();p1.Controls.AddRange(new Control[]{soTT,soDK,ngay,tien,ghiChu,Ui.Button("Ghi nhận thanh toán",ThanhToan),Ui.Button("Nạp danh sách",(s,e)=>grid.DataSource=svc.DoanCanThanhToan())});pay.Controls.Add(grid);pay.Controls.Add(p1);var p2=Ui.Top();p2.Controls.AddRange(new Control[]{maKS,loai,soKSDangKy,ngay,gopY,diem,Ui.Button("Gửi khảo sát",Gui),Ui.Button("Ghi phản hồi",PhanHoi),Ui.Button("Nạp danh sách",(s,e)=>grid.DataSource=svc.LayKhaoSat())});survey.Controls.Add(p2);tabs.TabPages.Add(pay);tabs.TabPages.Add(survey);Controls.Add(tabs);grid.DataSource=svc.DoanCanThanhToan();}
        private void ThanhToan(object s,EventArgs e){Ui.Show(svc.ThanhToanDoan(soTT.Text,soDK.Text,ngay.Value,tien.Value,ghiChu.Text));grid.DataSource=svc.DoanCanThanhToan();}
        private void Gui(object s,EventArgs e){Ui.Show(svc.GuiKhaoSat(maKS.Text,loai.Text,soKSDangKy.Text,ngay.Value));grid.DataSource=svc.LayKhaoSat();}
        private void PhanHoi(object s,EventArgs e){Ui.Show(svc.GhiPhanHoi(maKS.Text,ngay.Value,(int)diem.Value,gopY.Text));grid.DataSource=svc.LayKhaoSat();}
    }
}

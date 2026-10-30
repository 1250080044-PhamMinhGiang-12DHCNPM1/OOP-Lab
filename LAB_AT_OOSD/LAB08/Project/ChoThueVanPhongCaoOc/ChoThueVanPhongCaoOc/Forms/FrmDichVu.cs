using System;
using System.Data;
using System.Windows.Forms;
using ChoThueVanPhongCaoOc.Data;
using ChoThueVanPhongCaoOc.Services;

namespace ChoThueVanPhongCaoOc.Forms
{
    internal sealed class FrmDichVu : Form
    {
        private readonly ComboBox _cthd=new ComboBox{Width=190,DropDownStyle=ComboBoxStyle.DropDownList},_dv=new ComboBox{Width=170,DropDownStyle=ComboBoxStyle.DropDownList};
        private readonly NumericUpDown _thang=new NumericUpDown{Minimum=1,Maximum=12,Value=1},_nam=new NumericUpDown{Minimum=2000,Maximum=2100,Value=2026},_soLuong=new NumericUpDown{Minimum=1,Maximum=1000000,DecimalPlaces=2,Width=110};
        private readonly DataGridView _grid=new DataGridView{Dock=DockStyle.Fill,ReadOnly=true,AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill};
        internal FrmDichVu(){Text="Dịch vụ và hóa đơn cuối tháng";Width=1000;Height=560;StartPosition=FormStartPosition.CenterParent;var bar=new FlowLayoutPanel{Dock=DockStyle.Top,Height=65,Padding=new Padding(8)};Add(bar,"Chi tiết HĐ",_cthd);Add(bar,"Dịch vụ",_dv);Add(bar,"Tháng",_thang);Add(bar,"Năm",_nam);Add(bar,"Số lượng",_soLuong);var b=new Button{Text="Lập hóa đơn",Width=105,Margin=new Padding(8,18,3,3)};b.Click+=Save;bar.Controls.Add(b);Controls.Add(_grid);Controls.Add(bar);Load+=(s,e)=>Reload();}
        private static void Add(Control p,string l,Control c){var x=new FlowLayoutPanel{Width=c.Width+18,Height=55,FlowDirection=FlowDirection.TopDown};x.Controls.Add(new Label{Text=l,AutoSize=true});x.Controls.Add(c);p.Controls.Add(x);}
        private void Reload(){var c=Db.Query("SELECT MaCTHD,CAST(MaCTHD AS varchar)+' - '+MaHD+' - '+MaPhong HienThi FROM ChiTietHopDong ORDER BY MaCTHD DESC");_cthd.DataSource=c;_cthd.ValueMember="MaCTHD";_cthd.DisplayMember="HienThi";var d=Db.Query("SELECT MaDV,MaDV+' - '+TenDV HienThi FROM DichVu ORDER BY MaDV");_dv.DataSource=d;_dv.ValueMember="MaDV";_dv.DisplayMember="HienThi";_grid.DataSource=Db.Query("SELECT s.*,d.TenDV FROM SuDungDichVu s JOIN DichVu d ON d.MaDV=s.MaDV ORDER BY Nam DESC,Thang DESC");}
        private void Save(object s,EventArgs e){try{new RentalService().LapHoaDonDichVu(Convert.ToInt32(_cthd.SelectedValue),Convert.ToString(_dv.SelectedValue),(int)_thang.Value,(int)_nam.Value,_soLuong.Value);MessageBox.Show("Đã ghi nhận dịch vụ và tạo khoản thu cuối tháng.");Reload();}catch(Exception ex){MessageBox.Show(ex.Message,"Không thể lập hóa đơn");}}
    }
}

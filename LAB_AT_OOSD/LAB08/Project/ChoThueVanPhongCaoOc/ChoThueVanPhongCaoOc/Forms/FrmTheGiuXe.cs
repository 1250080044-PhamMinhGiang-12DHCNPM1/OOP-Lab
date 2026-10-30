using System;
using System.Windows.Forms;
using ChoThueVanPhongCaoOc.Data;
using ChoThueVanPhongCaoOc.Services;

namespace ChoThueVanPhongCaoOc.Forms
{
    internal sealed class FrmTheGiuXe : Form
    {
        private readonly TextBox _ma=new TextBox{Width=110},_bien=new TextBox{Width=120},_loai=new TextBox{Width=110};private readonly ComboBox _nv=new ComboBox{Width=200,DropDownStyle=ComboBoxStyle.DropDownList};private readonly DataGridView _grid=new DataGridView{Dock=DockStyle.Fill,ReadOnly=true,AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill};
        internal FrmTheGiuXe(){Text="Cấp thẻ giữ xe";Width=950;Height=520;StartPosition=FormStartPosition.CenterParent;var bar=new FlowLayoutPanel{Dock=DockStyle.Top,Height=58,Padding=new Padding(8)};bar.Controls.Add(new Label{Text="Mã thẻ",AutoSize=true,Margin=new Padding(3,9,3,3)});bar.Controls.Add(_ma);bar.Controls.Add(new Label{Text="Nhân viên",AutoSize=true,Margin=new Padding(8,9,3,3)});bar.Controls.Add(_nv);bar.Controls.Add(new Label{Text="Biển số",AutoSize=true,Margin=new Padding(8,9,3,3)});bar.Controls.Add(_bien);bar.Controls.Add(new Label{Text="Loại xe",AutoSize=true,Margin=new Padding(8,9,3,3)});bar.Controls.Add(_loai);var b=new Button{Text="Cấp thẻ"};b.Click+=Save;bar.Controls.Add(b);Controls.Add(_grid);Controls.Add(bar);Load+=(s,e)=>Reload();}
        private void Reload(){var n=Db.Query("SELECT MaNV,MaNV+' - '+HoTen HienThi FROM NhanVien ORDER BY MaNV");_nv.DataSource=n;_nv.ValueMember="MaNV";_nv.DisplayMember="HienThi";_grid.DataSource=Db.Query("SELECT t.MaThe,t.MaNV,n.HoTen,x.BienSo,x.LoaiXe,t.NgayCap,t.NgayHuy FROM TheGiuXe t JOIN NhanVien n ON n.MaNV=t.MaNV JOIN Xe x ON x.MaXe=t.MaXe ORDER BY t.NgayCap DESC");}
        private void Save(object s,EventArgs e){try{new RentalService().CapThe(_ma.Text.Trim(),Convert.ToString(_nv.SelectedValue),_bien.Text.Trim(),_loai.Text.Trim());MessageBox.Show("Đã cấp thẻ giữ xe.");Reload();}catch(Exception ex){MessageBox.Show(ex.Message,"Không thể cấp thẻ");}}
    }
}

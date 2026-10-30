using System;
using System.Windows.Forms;
using ChoThueVanPhongCaoOc.Data;
using ChoThueVanPhongCaoOc.Services;

namespace ChoThueVanPhongCaoOc.Forms
{
    internal sealed class FrmGiaHan : Form
    {
        private readonly ComboBox _cthd=new ComboBox{Width=260,DropDownStyle=ComboBoxStyle.DropDownList}; private readonly NumericUpDown _thang=new NumericUpDown{Minimum=1,Maximum=60,Value=1};
        private readonly DataGridView _grid=new DataGridView{Dock=DockStyle.Fill,ReadOnly=true,AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill};
        internal FrmGiaHan(){Text="Gia hạn hợp đồng";Width=920;Height=520;StartPosition=FormStartPosition.CenterParent;var bar=new FlowLayoutPanel{Dock=DockStyle.Top,Height=55,Padding=new Padding(8)};bar.Controls.Add(new Label{Text="Phòng đang thuê",AutoSize=true,Margin=new Padding(3,9,3,3)});bar.Controls.Add(_cthd);bar.Controls.Add(new Label{Text="Số tháng",AutoSize=true,Margin=new Padding(10,9,3,3)});bar.Controls.Add(_thang);var b=new Button{Text="Gia hạn",Width=90};b.Click+=Save;bar.Controls.Add(b);Controls.Add(_grid);Controls.Add(bar);Load+=(s,e)=>Reload();}
        private void Reload(){var d=Db.Query("SELECT c.MaCTHD,CAST(c.MaCTHD AS varchar)+' - '+c.MaHD+' - '+c.MaPhong+' - hết '+CONVERT(varchar,c.NgayKetThuc,103) HienThi FROM ChiTietHopDong c JOIN HopDong h ON h.MaHD=c.MaHD WHERE h.TrangThai IN(N'Chờ thanh toán',N'Hiệu lực')");_cthd.DataSource=d;_cthd.ValueMember="MaCTHD";_cthd.DisplayMember="HienThi";_grid.DataSource=Db.Query("SELECT g.*,c.MaHD,c.MaPhong FROM PhieuGiaHan g JOIN ChiTietHopDong c ON c.MaCTHD=g.MaCTHD ORDER BY g.MaGiaHan DESC");}
        private void Save(object s,EventArgs e){try{new RentalService().GiaHan(Convert.ToInt32(_cthd.SelectedValue),(int)_thang.Value);MessageBox.Show("Gia hạn thành công.");Reload();}catch(Exception ex){MessageBox.Show(ex.Message,"Không thể gia hạn");}}
    }
}

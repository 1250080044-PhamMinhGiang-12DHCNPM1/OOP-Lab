using System;
using System.Windows.Forms;
using ChoThueVanPhongCaoOc.Data;
using ChoThueVanPhongCaoOc.Services;

namespace ChoThueVanPhongCaoOc.Forms
{
    internal sealed class FrmThanhToan : Form
    {
        private readonly DataGridView _grid=new DataGridView{Dock=DockStyle.Fill,ReadOnly=true,SelectionMode=DataGridViewSelectionMode.FullRowSelect,AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill};
        private readonly NumericUpDown _tien=new NumericUpDown{Maximum=1000000000,ThousandsSeparator=true,Width=150};
        private readonly TextBox _ghiChu=new TextBox{Width=220};
        internal FrmThanhToan(){Text="Thanh toán và công nợ";Width=1000;Height=560;StartPosition=FormStartPosition.CenterParent;var bar=new FlowLayoutPanel{Dock=DockStyle.Top,Height=55,Padding=new Padding(8)};bar.Controls.Add(new Label{Text="Số tiền",AutoSize=true,Margin=new Padding(3,9,3,3)});bar.Controls.Add(_tien);bar.Controls.Add(new Label{Text="Ghi chú",AutoSize=true,Margin=new Padding(12,9,3,3)});bar.Controls.Add(_ghiChu);var b=new Button{Text="Ghi nhận",Width=100};b.Click+=Save;bar.Controls.Add(b);Controls.Add(_grid);Controls.Add(bar);Load+=(s,e)=>Reload();}
        private void Reload(){_grid.DataSource=Db.Query("SELECT * FROM dbo.vw_CongNo WHERE ConNo>0 ORDER BY HanThanhToan");}
        private void Save(object s,EventArgs e){if(_grid.CurrentRow==null)return;try{new RentalService().ThanhToan(Convert.ToInt32(_grid.CurrentRow.Cells["MaKhoanThu"].Value),_tien.Value,_ghiChu.Text.Trim());MessageBox.Show("Đã ghi nhận thanh toán.");Reload();}catch(Exception ex){MessageBox.Show(ex.Message,"Không thể thanh toán");}}
    }
}

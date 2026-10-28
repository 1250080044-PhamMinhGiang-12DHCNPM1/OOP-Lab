using System;
using System.Windows.Forms;
using QuanLyCongTyDuLich.Services;

namespace QuanLyCongTyDuLich.Forms
{
    public class FrmLuongThongKe : Form
    {
        private readonly ThongKeService svc=new ThongKeService();private readonly DataGridView grid=Ui.Grid();private readonly NumericUpDown thang=Ui.Number(DateTime.Today.Month,12),nam=Ui.Number(DateTime.Today.Year,2100);private readonly DateTimePicker tu=new DateTimePicker{Value=new DateTime(DateTime.Today.Year,1,1),Width=135,Margin=new Padding(4)},den=new DateTimePicker{Value=DateTime.Today,Width=135,Margin=new Padding(4)};
        public FrmLuongThongKe(){Text="Lương hướng dẫn viên và thống kê";Width=920;Height=540;var top=Ui.Top();top.Controls.AddRange(new Control[]{thang,nam,Ui.Button("Tính lương",Luong),tu,den,Ui.Button("Thống kê",ThongKe)});Controls.Add(grid);Controls.Add(top);}
        private void Luong(object s,EventArgs e){grid.DataSource=svc.LuongHDV((int)thang.Value,(int)nam.Value);}
        private void ThongKe(object s,EventArgs e){if(den.Value.Date<tu.Value.Date){MessageBox.Show("Đến ngày không được trước từ ngày.");return;}grid.DataSource=svc.TongHop(tu.Value,den.Value);}
    }
}

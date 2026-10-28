using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using QuanLyCongTyDuLich.Services;

namespace QuanLyCongTyDuLich.Forms
{
    public class FrmDangKyDoan : Form
    {
        private readonly DangKyDoanService svc=new DangKyDoanService();private readonly DataGridView grid=Ui.Grid();private readonly TextBox so=Ui.Text("Số phiếu"),maDoan=Ui.Text("Mã đoàn"),coQuan=Ui.Text("Cơ quan/gia đình",180),diaChi=Ui.Text("Địa chỉ",180),dienThoai=Ui.Text("Điện thoại"),daiDien=Ui.Text("Đại diện"),tour=Ui.Text("Mã tour"),don=Ui.Text("Điểm đón",170),ds=Ui.Text("DS tên, cách dấu phẩy",230);private readonly NumericUpDown nguoi=Ui.Number(13,999),coc=Ui.Number(1000000,999999999);private readonly DateTimePicker ngay=new DateTimePicker { Value=DateTime.Today.AddDays(45),Width=135,Margin=new Padding(4)};private readonly CheckBox baoHiem=new CheckBox{Text="Mua bảo hiểm",AutoSize=true,Margin=new Padding(8)};
        public FrmDangKyDoan(){Text="Đăng ký tour theo đoàn";Width=1220;Height=680;var top=Ui.Top();top.Controls.AddRange(new Control[]{so,maDoan,coQuan,diaChi,dienThoai,daiDien,tour,ngay,nguoi,don,coc,baoHiem,ds,Ui.Button("Lập phiếu",DangKy),Ui.Button("Hủy phiếu chọn",Huy),Ui.Button("Nạp lại",(s,e)=>LoadData())});Controls.Add(grid);Controls.Add(top);LoadData();}
        private void LoadData(){grid.DataSource=svc.LayDanhSach();}
        private void DangKy(object s,EventArgs e){var names=ds.Text.Split(new[]{','},StringSplitOptions.RemoveEmptyEntries).Select(x=>x.Trim()).ToList();Ui.Show(svc.DangKy(so.Text,maDoan.Text,coQuan.Text,diaChi.Text,dienThoai.Text,daiDien.Text,tour.Text,ngay.Value,(int)nguoi.Value,don.Text,baoHiem.Checked,coc.Value,names));LoadData();}
        private void Huy(object s,EventArgs e){Ui.Show(svc.HuyDangKy(Ui.Cell(grid,"SoDKDoan")));LoadData();}
    }
}

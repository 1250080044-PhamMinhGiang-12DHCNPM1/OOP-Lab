using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyCongTyDuLich.Forms
{
    public class FrmMain : Form
    {
        public FrmMain()
        {
            Text="Quản lý Công ty Du lịch Văn Hóa Việt"; Width=1120; Height=700; StartPosition=FormStartPosition.CenterScreen; Font=new Font("Segoe UI",10);
            var menu=new FlowLayoutPanel { Dock=DockStyle.Left, Width=245, Padding=new Padding(12), FlowDirection=FlowDirection.TopDown, BackColor=Color.FromArgb(31,78,120) };
            var title=new Label { Text="VĂN HÓA VIỆT\nQUẢN LÝ TOUR", ForeColor=Color.White, Font=new Font("Segoe UI",15,FontStyle.Bold), Height=90, TextAlign=ContentAlignment.MiddleCenter, Width=215 };
            menu.Controls.Add(title);
            Add(menu,"Danh mục",delegate { new FrmDanhMuc().ShowDialog(); }); Add(menu,"Tour - hành trình",delegate { new FrmTour().ShowDialog(); }); Add(menu,"Lịch chuyến lẻ",delegate { new FrmChuyenLe().ShowDialog(); }); Add(menu,"Đăng ký khách lẻ",delegate { new FrmDangKyLe().ShowDialog(); }); Add(menu,"Đăng ký đoàn",delegate { new FrmDangKyDoan().ShowDialog(); }); Add(menu,"Phân công HDV",delegate { new FrmPhanCongHDV().ShowDialog(); }); Add(menu,"Kết thúc - khảo sát",delegate { new FrmKetThucKhaoSat().ShowDialog(); }); Add(menu,"Lương - thống kê",delegate { new FrmLuongThongKe().ShowDialog(); });
            var info=new Label { Dock=DockStyle.Fill, Text="HỆ THỐNG QUẢN LÝ CÔNG TY DU LỊCH\n\nChọn một chức năng ở thanh điều hướng bên trái.\n\nKiến trúc: Forms  →  Services  →  Data/Db.cs  →  SQL Server", TextAlign=ContentAlignment.MiddleCenter, Font=new Font("Segoe UI",16), ForeColor=Color.FromArgb(31,78,120) };
            Controls.Add(info); Controls.Add(menu);
        }
        private void Add(FlowLayoutPanel p,string text,EventHandler action) { var b=new Button { Text=text, Width=215, Height=42, FlatStyle=FlatStyle.Flat, ForeColor=Color.White, BackColor=Color.FromArgb(31,78,120), TextAlign=ContentAlignment.MiddleLeft }; b.FlatAppearance.BorderColor=Color.FromArgb(90,140,180); b.Click+=action; p.Controls.Add(b); }
    }
}

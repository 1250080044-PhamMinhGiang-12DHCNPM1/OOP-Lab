using System;
using System.Windows.Forms;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.Forms
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            Load += FrmMain_Load;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            lblNguoiDung.Text = PhienDangNhap.HoTen + " - " + PhienDangNhap.VaiTro;
            ApDungPhanQuyen();
        }

        private void ApDungPhanQuyen()
        {
            string role = PhienDangNhap.VaiTro ?? "";
            if (role == "Quản lý") return;

            btnDanhMuc.Enabled = false;
            btnThongKe.Enabled = false;

            if (role == "Lễ tân")
            {
                btnPhong.Enabled = false;
                btnDichVu.Enabled = false;
                btnTraPhong.Enabled = false;
            }
            else if (role == "Phục vụ phòng")
            {
                btnDatPhong.Enabled = false;
                btnDichVu.Enabled = false;
            }
            else if (role == "Nhân viên dịch vụ")
            {
                btnDanhMuc.Enabled = false;
                btnPhong.Enabled = false;
                btnDatPhong.Enabled = false;
                btnTraPhong.Enabled = false;
            }
            else if (role == "Thanh toán")
            {
                btnDanhMuc.Enabled = false;
                btnPhong.Enabled = false;
                btnDatPhong.Enabled = false;
                btnDichVu.Enabled = false;
            }
        }

        private void Open(Form form)
        {
            using (form) form.ShowDialog(this);
        }

        private void btnDanhMuc_Click(object sender, EventArgs e) { Open(new FrmDanhMuc()); }
        private void btnPhong_Click(object sender, EventArgs e) { Open(new FrmPhongTienNghi()); }
        private void btnDatPhong_Click(object sender, EventArgs e) { Open(new FrmDatPhong()); }
        private void btnDichVu_Click(object sender, EventArgs e) { Open(new FrmDichVu()); }
        private void btnTraPhong_Click(object sender, EventArgs e) { Open(new FrmTraPhong()); }
        private void btnThongKe_Click(object sender, EventArgs e) { Open(new FrmThongKe()); }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            PhienDangNhap.Xoa();
            Close();
        }
    }
}

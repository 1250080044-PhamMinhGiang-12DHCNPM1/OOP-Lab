using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Services;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private readonly DichVuMuaHang _dichVuMuaHang = new DichVuMuaHang();
        private readonly List<MucGioHang> _gioHang = new List<MucGioHang>();
        private DataGridView _luoiSanPham;
        private ListBox _danhSachGio;
        private Label _nhanTongTien;
        private NumericUpDown _soLuong;
        private TextBox _hoTen, _soDienThoai, _diaChi, _thuDienTu, _soThe;

        public Form1()
        {
            InitializeComponent();
            KhoiTaoGiaoDien();
            Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                _luoiSanPham.DataSource = _dichVuMuaHang.LaySanPham();
                DinhDangLuoiSanPham();
            }
            catch (Exception loi)
            {
                MessageBox.Show("Chưa kết nối được CSDL. Hãy chạy tệp Database\\01_Tao_CSDL_eShopping.sql rồi kiểm tra chuỗi kết nối.\n\n" + loi.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void KhoiTaoGiaoDien()
        {
            Text = "e-SHOPPING - Prototype";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1120, 650);
            Font = new Font("Segoe UI", 9F);
            Controls.Add(new Label { Text = "E-SHOPPING - ĐẶT HÀNG TRỰC TUYẾN", Font = new Font("Segoe UI", 16F, FontStyle.Bold), AutoSize = true, Location = new Point(30, 20) });

            _luoiSanPham = new DataGridView { Location = new Point(30, 70), Size = new Size(545, 260), ReadOnly = true, AutoGenerateColumns = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, AllowUserToAddRows = false };
            Controls.Add(_luoiSanPham);
            Controls.Add(new Label { Text = "Số lượng:", Location = new Point(30, 350), AutoSize = true });
            _soLuong = new NumericUpDown { Location = new Point(100, 347), Minimum = 1, Maximum = 20, Value = 1, Width = 60 };
            Controls.Add(_soLuong);
            var nutThem = new Button { Text = "Thêm vào giỏ", Location = new Point(180, 344), Size = new Size(130, 32) };
            nutThem.Click += NutThem_Click;
            Controls.Add(nutThem);

            Controls.Add(new Label { Text = "GIỎ HÀNG", Font = new Font("Segoe UI", 11F, FontStyle.Bold), AutoSize = true, Location = new Point(620, 70) });
            _danhSachGio = new ListBox { Location = new Point(620, 100), Size = new Size(455, 190) };
            Controls.Add(_danhSachGio);
            _nhanTongTien = new Label { Text = "Tổng tiền: 0 đ", Font = new Font("Segoe UI", 10F, FontStyle.Bold), AutoSize = true, Location = new Point(620, 305) };
            Controls.Add(_nhanTongTien);

            var nhom = new GroupBox { Text = "Thông tin nhận hàng và thanh toán", Location = new Point(30, 410), Size = new Size(1045, 205) };
            _hoTen = TaoOText(nhom, "Họ tên người nhận:", 20, 30);
            _soDienThoai = TaoOText(nhom, "Số điện thoại:", 20, 75);
            _diaChi = TaoOText(nhom, "Địa chỉ giao hàng:", 20, 120);
            _thuDienTu = TaoOText(nhom, "Thư điện tử (tùy chọn):", 520, 30);
            _soThe = TaoOText(nhom, "Số thẻ (ít nhất 6 số):", 520, 75);
            var nutDatHang = new Button { Text = "Xác nhận đặt hàng", Location = new Point(760, 120), Size = new Size(190, 40), BackColor = Color.LightGreen };
            nutDatHang.Click += NutDatHang_Click;
            nhom.Controls.Add(nutDatHang);
            Controls.Add(nhom);
        }

        private TextBox TaoOText(Control cha, string nhan, int x, int y)
        {
            cha.Controls.Add(new Label { Text = nhan, Location = new Point(x, y + 4), AutoSize = true });
            var oText = new TextBox { Location = new Point(x + 170, y), Width = 260 };
            cha.Controls.Add(oText);
            return oText;
        }

        private void DinhDangLuoiSanPham()
        {
            if (_luoiSanPham.Columns.Count == 0) return;
            _luoiSanPham.Columns["MaSanPham"].Visible = false;
            _luoiSanPham.Columns["MaHienThi"].HeaderText = "Mã";
            _luoiSanPham.Columns["TenSanPham"].HeaderText = "Tên sản phẩm";
            _luoiSanPham.Columns["DonGia"].HeaderText = "Đơn giá";
            _luoiSanPham.Columns["SoLuongTon"].HeaderText = "Tồn kho";
            _luoiSanPham.Columns["DonGia"].DefaultCellStyle.Format = "N0";
        }

        private void NutThem_Click(object sender, EventArgs e)
        {
            var sanPham = _luoiSanPham.CurrentRow == null ? null : _luoiSanPham.CurrentRow.DataBoundItem as SanPham;
            if (sanPham == null) { MessageBox.Show("Hãy chọn sản phẩm."); return; }
            var muc = _gioHang.FirstOrDefault(x => x.SanPham.MaSanPham == sanPham.MaSanPham);
            if (muc == null) _gioHang.Add(new MucGioHang { SanPham = sanPham, SoLuong = (int)_soLuong.Value });
            else muc.SoLuong += (int)_soLuong.Value;
            HienThiGioHang();
        }

        private void HienThiGioHang()
        {
            _danhSachGio.Items.Clear();
            foreach (var muc in _gioHang) _danhSachGio.Items.Add(string.Format("{0} x {1} = {2:N0} đ", muc.SanPham.TenSanPham, muc.SoLuong, muc.ThanhTien));
            _nhanTongTien.Text = string.Format("Tổng tiền: {0:N0} đ", _gioHang.Sum(x => x.ThanhTien));
        }

        private void NutDatHang_Click(object sender, EventArgs e)
        {
            try
            {
                var nguoiNhan = new NguoiNhan { HoTen = _hoTen.Text, SoDienThoai = _soDienThoai.Text, DiaChi = _diaChi.Text, HinhThucGiaoHang = "Giao hàng thường", ThuDienTu = _thuDienTu.Text };
                int maDonHang = _dichVuMuaHang.DatHang(1, _gioHang, nguoiNhan, _soThe.Text);
                MessageBox.Show("Đặt hàng thành công. Mã đơn hàng: " + maDonHang, "e-SHOPPING", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _gioHang.Clear();
                HienThiGioHang();
            }
            catch (Exception loi)
            {
                MessageBox.Show(loi.Message, "Không thể đặt hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ChoThueVanPhongCaoOc.Data;
using ChoThueVanPhongCaoOc.Services;

namespace ChoThueVanPhongCaoOc.Forms
{
    internal sealed class FrmHopDong : Form
    {
        private readonly TextBox _ma = new TextBox { Width = 120 };
        private readonly ComboBox _kh = new ComboBox { Width = 210, DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly ComboBox _phong = new ComboBox { Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly DateTimePicker _batDau = new DateTimePicker { Width = 130 };
        private readonly NumericUpDown _thang = new NumericUpDown { Minimum = 1, Maximum = 120, Value = 6, Width = 70 };
        private readonly NumericUpDown _gia = new NumericUpDown { Maximum = 1000000000, DecimalPlaces = 0, ThousandsSeparator = true, Width = 140 };
        private readonly DataGridView _grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        private readonly RentalService _service = new RentalService();

        internal FrmHopDong()
        {
            Text = "Lập hợp đồng thuê"; Width = 1050; Height = 600; StartPosition = FormStartPosition.CenterParent;
            var bar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 105, Padding = new Padding(8), AutoScroll = true };
            Add(bar,"Mã HĐ",_ma); Add(bar,"Khách hàng",_kh); Add(bar,"Phòng",_phong); Add(bar,"Ngày bắt đầu",_batDau); Add(bar,"Số tháng",_thang); Add(bar,"Đơn giá",_gia);
            var save = new Button { Text = "Lập hợp đồng", Width = 120, Height = 30, Margin = new Padding(8, 22, 3, 3) }; save.Click += Save; bar.Controls.Add(save);
            Controls.Add(_grid); Controls.Add(bar); Load += (s,e) => LoadData();
        }

        private static void Add(Control p,string label,Control c){var x=new FlowLayoutPanel{Width=Math.Max(c.Width+20,100),Height=72,FlowDirection=FlowDirection.TopDown};x.Controls.Add(new Label{Text=label,AutoSize=true});x.Controls.Add(c);p.Controls.Add(x);}
        private void LoadData()
        {
            var kh=Db.Query("SELECT MaKH,MaKH+' - '+TenKH HienThi FROM KhachHang ORDER BY MaKH"); _kh.DataSource=kh; _kh.ValueMember="MaKH"; _kh.DisplayMember="HienThi";
            var p=Db.Query("SELECT MaPhong,MaPhong+' - '+CAST(GiaThue AS varchar(30)) HienThi,GiaThue FROM Phong WHERE TrangThai=N'Trống' ORDER BY MaPhong"); _phong.DataSource=p; _phong.ValueMember="MaPhong"; _phong.DisplayMember="HienThi";
            _phong.SelectedIndexChanged += (s,e) => { if(_phong.SelectedItem is DataRowView r) _gia.Value=Convert.ToDecimal(r["GiaThue"]); };
            if(_phong.SelectedItem is DataRowView first) _gia.Value=Convert.ToDecimal(first["GiaThue"]);
            _grid.DataSource=Db.Query("SELECT h.*,c.MaPhong,c.DonGiaThue,c.NgayKetThuc FROM HopDong h JOIN ChiTietHopDong c ON c.MaHD=h.MaHD ORDER BY h.NgayKy DESC");
        }
        private void Save(object sender,EventArgs e){try{_service.LapHopDong(_ma.Text.Trim(),Convert.ToString(_kh.SelectedValue),DateTime.Today,_batDau.Value,(int)_thang.Value,Convert.ToString(_phong.SelectedValue),_gia.Value);MessageBox.Show("Đã lập hợp đồng và tạo khoản thu 6 tháng đầu.");LoadData();}catch(Exception ex){MessageBox.Show(ex.Message,"Không thể lập hợp đồng");}}
    }
}

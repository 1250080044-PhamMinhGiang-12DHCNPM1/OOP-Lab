using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using QuanLyNhanXu.Models;
using QuanLyNhanXu.Services;

namespace QuanLyNhanXu.Forms
{
    public sealed class FrmMain : Form
    {
        private readonly TuyenDungService _tuyenDung = new TuyenDungService();
        private readonly HopDongBaoHiemService _hopDong = new HopDongBaoHiemService();
        private readonly QuyetDinhService _quyetDinh = new QuyetDinhService();
        private readonly KeHoachChamCongService _chamCong = new KeHoachChamCongService();
        private readonly KhenThuongKyLuatService _thuongPhat = new KhenThuongKyLuatService();
        private readonly BangLuongService _bangLuong = new BangLuongService();

        public FrmMain()
        {
            Text = "Quản lý nhân sự Công ty Sơn Đông Á";
            Width = 1260; Height = 780; StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 9F); BackColor = Color.White;
            var header = new Label { Text = "QUẢN LÝ NHÂN SỰ CÔNG TY SƠN ĐÔNG Á", Dock = DockStyle.Top, Height = 54, TextAlign = ContentAlignment.MiddleCenter, BackColor = Color.FromArgb(25, 78, 121), ForeColor = Color.White, Font = new Font("Segoe UI", 17F, FontStyle.Bold) };
            var tabs = new TabControl { Dock = DockStyle.Fill };
            tabs.TabPages.Add(TaoTabTuyenDung()); tabs.TabPages.Add(TaoTabHopDong()); tabs.TabPages.Add(TaoTabQuyetDinh());
            tabs.TabPages.Add(TaoTabKeHoach()); tabs.TabPages.Add(TaoTabChamCong()); tabs.TabPages.Add(TaoTabThuongPhat()); tabs.TabPages.Add(TaoTabBangLuong());
            Controls.Add(tabs); Controls.Add(header);
        }

        private TabPage TaoTabTuyenDung()
        {
            var page = NewPage("1. Tuyển dụng - Thử việc"); var layout = NewLayout(); var grid = NewGrid();
            var hoTen = AddText(layout, "Họ tên", 0); var cccd = AddText(layout, "CCCD", 1); var dienThoai = AddText(layout, "Điện thoại", 2); var diaChi = AddText(layout, "Địa chỉ", 3);
            var ngaySinh = AddDate(layout, "Ngày sinh", 4, new DateTime(2000, 1, 1)); var gioiTinh = AddCombo(layout, "Giới tính", 5, "Nam", "Nữ", "Khác");
            var phong = AddNumber(layout, "Mã phòng ban", 6, 1, 1, 100); var nhuCau = AddNumber(layout, "Mã nhu cầu", 7, 1, 0, 1000);
            var tuNgay = AddDate(layout, "Từ ngày", 8, DateTime.Today); var denNgay = AddDate(layout, "Đến ngày thử việc", 9, DateTime.Today.AddMonths(2));
            var luong = AddMoney(layout, "Lương thử việc", 10, 8000000); var dieuKhoan = AddText(layout, "Điều khoản", 11);
            var actions = NewActions(); var create = NewButton("Tiếp nhận thử việc"); var pass = NewButton("Đánh giá đạt"); var fail = NewButton("Không đạt"); var maNV = NewSmallText("Mã NV đánh giá");
            actions.Controls.Add(create); actions.Controls.Add(maNV); actions.Controls.Add(pass); actions.Controls.Add(fail); layout.Controls.Add(actions, 1, 12);
            create.Click += delegate { Run(delegate { string code = _tuyenDung.TiepNhanThuViec(new ThuViecInput { HoTen = hoTen.Text, NgaySinh = ngaySinh.Value, GioiTinh = gioiTinh.Text, CCCD = cccd.Text, DienThoai = dienThoai.Text, DiaChi = diaChi.Text, MaPhongBan = (int)phong.Value, MaNhuCau = nhuCau.Value == 0 ? (int?)null : (int)nhuCau.Value, TuNgay = tuNgay.Value, DenNgayThuViec = denNgay.Value, LuongThuViec = luong.Value, DieuKhoan = dieuKhoan.Text }); maNV.Text = code; grid.DataSource = _tuyenDung.DanhSachNhanVien(); return "Đã tạo quyết định thử việc cho " + code; }); };
            pass.Click += delegate { Run(delegate { _tuyenDung.DanhGiaThuViec(maNV.Text, true); grid.DataSource = _tuyenDung.DanhSachNhanVien(); return "Đã ghi nhận kết quả đạt."; }); };
            fail.Click += delegate { Run(delegate { _tuyenDung.DanhGiaThuViec(maNV.Text, false); grid.DataSource = _tuyenDung.DanhSachNhanVien(); return "Đã ghi nhận kết quả không đạt."; }); };
            grid.DataSource = SafeLoad(_tuyenDung.DanhSachNhanVien); return FinishPage(page, layout, grid);
        }

        private TabPage TaoTabHopDong()
        {
            var page = NewPage("2. Hợp đồng - Bảo hiểm"); var layout = NewLayout(); var grid = NewGrid();
            var maNV = AddText(layout, "Mã nhân viên", 0); var loai = AddCombo(layout, "Loại hợp đồng", 1, "Ngan han", "Dai han");
            var ngayKy = AddDate(layout, "Ngày ký", 2, DateTime.Today); var tuNgay = AddDate(layout, "Từ ngày", 3, DateTime.Today); var denNgay = AddDate(layout, "Đến ngày", 4, DateTime.Today.AddYears(1));
            var soBHXH = AddText(layout, "Số BHXH", 5); var soBHYT = AddText(layout, "Số BHYT", 6); var actions = NewActions(); var sign = NewButton("Ký hợp đồng chính thức"); var insurance = NewButton("Tạo hồ sơ bảo hiểm"); actions.Controls.Add(sign); actions.Controls.Add(insurance); layout.Controls.Add(actions, 1, 7);
            sign.Click += delegate { Run(delegate { int id = _hopDong.KyHopDongChinhThuc(new HopDongInput { MaNV = maNV.Text, LoaiHopDong = loai.Text, NgayKy = ngayKy.Value, TuNgay = tuNgay.Value, DenNgay = loai.Text == "Dai han" ? (DateTime?)null : denNgay.Value }); grid.DataSource = _hopDong.DanhSachHopDong(); return "Đã ký hợp đồng số " + id; }); };
            insurance.Click += delegate { Run(delegate { _hopDong.TaoHoSoBaoHiem(maNV.Text, soBHXH.Text, soBHYT.Text, tuNgay.Value); grid.DataSource = _hopDong.DanhSachHopDong(); return "Đã tạo hồ sơ BHXH và BHYT."; }); };
            grid.DataSource = SafeLoad(_hopDong.DanhSachHopDong); return FinishPage(page, layout, grid);
        }

        private TabPage TaoTabQuyetDinh()
        {
            var page = NewPage("3. Lương - Chức vụ"); var layout = NewLayout(); var grid = NewGrid();
            var maNV = AddText(layout, "Mã nhân viên", 0); var hieuLuc = AddDate(layout, "Ngày hiệu lực", 1, DateTime.Today);
            var luong = AddMoney(layout, "Lương cơ bản", 2, 12000000); var pc = AddMoney(layout, "Phụ cấp lương", 3, 500000); var dh = AddMoney(layout, "Phụ cấp độc hại", 4, 300000);
            var chucVu = AddNumber(layout, "Mã chức vụ", 5, 2, 1, 100); var pcChucVu = AddMoney(layout, "Phụ cấp chức vụ", 6, 1500000); var actions = NewActions(); var salary = NewButton("Ban hành QĐ lương"); var position = NewButton("Ban hành QĐ chức vụ"); var search = NewButton("Xem lịch sử"); actions.Controls.Add(salary); actions.Controls.Add(position); actions.Controls.Add(search); layout.Controls.Add(actions, 1, 7);
            salary.Click += delegate { Run(delegate { _quyetDinh.BanHanhQuyetDinhLuong(maNV.Text, hieuLuc.Value, luong.Value, pc.Value, dh.Value); grid.DataSource = _quyetDinh.LichSu(maNV.Text); return "Đã ban hành quyết định lương."; }); };
            position.Click += delegate { Run(delegate { _quyetDinh.BanHanhQuyetDinhChucVu(maNV.Text, (int)chucVu.Value, hieuLuc.Value, pcChucVu.Value); grid.DataSource = _quyetDinh.LichSu(maNV.Text); return "Đã ban hành quyết định chức vụ."; }); };
            search.Click += delegate { grid.DataSource = SafeLoad(delegate { return _quyetDinh.LichSu(maNV.Text); }); }; return FinishPage(page, layout, grid);
        }

        private TabPage TaoTabKeHoach()
        {
            var page = NewPage("4. Kế hoạch tuần"); var layout = NewLayout(); var grid = NewGrid();
            var phong = AddNumber(layout, "Mã phòng ban", 0, 1, 1, 100); var thuHai = AddDate(layout, "Thứ Hai đầu tuần", 1, MondayOf(DateTime.Today));
            var nguoiLap = AddText(layout, "Người lập", 2); nguoiLap.Text = "Trưởng phòng"; var maNV = AddText(layout, "Mã nhân viên", 3);
            var ngayLam = AddDate(layout, "Ngày làm", 4, MondayOf(DateTime.Today)); var ca = AddNumber(layout, "Mã ca", 5, 1, 1, 100); var duKien = new CheckBox { Text = "Dự kiến phụ trội", AutoSize = true }; layout.Controls.Add(duKien, 1, 6); var add = NewButton("Thêm phân công tuần"); layout.Controls.Add(add, 1, 7);
            add.Click += delegate { Run(delegate { _chamCong.LapKeHoachTuan((int)phong.Value, thuHai.Value, nguoiLap.Text, maNV.Text, ngayLam.Value, (int)ca.Value, duKien.Checked); grid.DataSource = QuanLyNhanXu.Data.Db.Query("SELECT k.TuNgay,k.DenNgay,c.MaNV,c.NgayLam,ca.TenCa,c.DuKienPhuTroi FROM ChiTietKeHoach c JOIN KeHoachTuan k ON k.MaKeHoach=c.MaKeHoach JOIN CaLamViec ca ON ca.MaCa=c.MaCa ORDER BY c.NgayLam DESC"); return "Đã thêm phân công."; }); };
            grid.DataSource = SafeLoad(delegate { return QuanLyNhanXu.Data.Db.Query("SELECT TOP 100 k.TuNgay,k.DenNgay,c.MaNV,c.NgayLam,ca.TenCa,c.DuKienPhuTroi FROM ChiTietKeHoach c JOIN KeHoachTuan k ON k.MaKeHoach=c.MaKeHoach JOIN CaLamViec ca ON ca.MaCa=c.MaCa ORDER BY c.NgayLam DESC"); }); return FinishPage(page, layout, grid);
        }

        private TabPage TaoTabChamCong()
        {
            var page = NewPage("5. Chấm công - Phụ trội"); var layout = NewLayout(); var grid = NewGrid();
            var maNV = AddText(layout, "Mã nhân viên", 0); var ngay = AddDate(layout, "Ngày", 1, DateTime.Today); var ca = AddNumber(layout, "Mã ca", 2, 1, 1, 100);
            var ngayCong = AddNumber(layout, "Số ngày công", 3, 1, 0, 1, 2); var trangThai = AddCombo(layout, "Trạng thái", 4, "Co mat", "Nghi phep", "Vang");
            var hinhThuc = AddCombo(layout, "Hình thức phụ trội", 5, "Tang ca", "Ngay le", "Ngay nghi"); var soGio = AddNumber(layout, "Số giờ phụ trội", 6, 2, 0, 24, 2);
            var thang = AddNumber(layout, "Tháng", 7, DateTime.Today.Month, 1, 12); var nam = AddNumber(layout, "Năm", 8, DateTime.Today.Year, 2020, 2100);
            var actions = NewActions(); var attend = NewButton("Ghi nhận chấm công"); var overtime = NewButton("Lập phiếu phụ trội"); var close = NewButton("Chốt bảng công"); var view = NewButton("Xem bảng công"); actions.Controls.Add(attend); actions.Controls.Add(overtime); actions.Controls.Add(close); actions.Controls.Add(view); layout.Controls.Add(actions, 1, 9);
            attend.Click += delegate { Run(delegate { _chamCong.GhiNhanChamCong(maNV.Text, ngay.Value, (int)ca.Value, ngayCong.Value, trangThai.Text); grid.DataSource = _chamCong.XemChamCong((int)thang.Value, (int)nam.Value); return "Đã ghi nhận chấm công."; }); };
            overtime.Click += delegate { Run(delegate { _chamCong.LapPhieuPhuTroi(maNV.Text, ngay.Value, hinhThuc.Text, soGio.Value, "Ban chấm công"); return "Đã lập phiếu phụ trội hệ số 1,5."; }); };
            close.Click += delegate { Run(delegate { _chamCong.ChotChamCong((int)thang.Value, (int)nam.Value); return "Đã chốt bảng chấm công."; }); };
            view.Click += delegate { grid.DataSource = SafeLoad(delegate { return _chamCong.XemChamCong((int)thang.Value, (int)nam.Value); }); }; return FinishPage(page, layout, grid);
        }

        private TabPage TaoTabThuongPhat()
        {
            var page = NewPage("6. Khen thưởng - Kỷ luật"); var layout = NewLayout(); var grid = NewGrid();
            var doiTuong = AddCombo(layout, "Đối tượng", 0, "Cá nhân", "Tập thể"); var maNV = AddText(layout, "Mã nhân viên", 1); var phong = AddNumber(layout, "Mã phòng ban", 2, 1, 1, 100);
            var ngay = AddDate(layout, "Ngày quyết định", 3, DateTime.Today); var lyDo = AddText(layout, "Lý do", 4); var tien = AddMoney(layout, "Số tiền", 5, 1000000);
            var hinhThuc = AddCombo(layout, "Hình thức", 6, "Cong luong", "Tien mat", "Tru luong", "Tru truc tiep"); var nguoiKy = AddText(layout, "Người ký", 7); nguoiKy.Text = "Giám đốc";
            var actions = NewActions(); var reward = NewButton("Lập quyết định khen thưởng"); var discipline = NewButton("Lập quyết định kỷ luật"); actions.Controls.Add(reward); actions.Controls.Add(discipline); layout.Controls.Add(actions, 1, 8);
            Func<QuyetDinhTaiChinhInput> input = delegate { return new QuyetDinhTaiChinhInput { DoiTuong = doiTuong.Text, MaNV = maNV.Text, MaPhongBan = (int)phong.Value, Ngay = ngay.Value, LyDo = lyDo.Text, SoTien = tien.Value, HinhThuc = hinhThuc.Text, NguoiKy = nguoiKy.Text }; };
            reward.Click += delegate { Run(delegate { _thuongPhat.LapQuyetDinhKhenThuong(input()); grid.DataSource = _thuongPhat.DanhSach(); return "Đã lập quyết định và phiếu chi khen thưởng."; }); };
            discipline.Click += delegate { Run(delegate { _thuongPhat.LapQuyetDinhKyLuat(input()); grid.DataSource = _thuongPhat.DanhSach(); return "Đã lập quyết định và phiếu thu nộp phạt."; }); };
            grid.DataSource = SafeLoad(_thuongPhat.DanhSach); return FinishPage(page, layout, grid);
        }

        private TabPage TaoTabBangLuong()
        {
            var page = NewPage("7. Bảng lương"); var layout = NewLayout(); var grid = NewGrid();
            var thang = AddNumber(layout, "Tháng", 0, DateTime.Today.Month, 1, 12); var nam = AddNumber(layout, "Năm", 1, DateTime.Today.Year, 2020, 2100);
            var formula = new Label { AutoSize = true, MaximumSize = new Size(530, 0), Text = "Thực lĩnh = lương theo công + phụ cấp + phụ trội + thưởng - phạt - BHXH - BHYT. Tham số mặc định: 26 ngày, 8 giờ/ngày, hệ số 1,5." }; layout.Controls.Add(formula, 1, 2);
            var actions = NewActions(); var calculate = NewButton("Tính lương tháng"); var close = NewButton("Chốt bảng lương"); var view = NewButton("Xem chi tiết"); actions.Controls.Add(calculate); actions.Controls.Add(close); actions.Controls.Add(view); layout.Controls.Add(actions, 1, 3);
            calculate.Click += delegate { Run(delegate { _bangLuong.TinhLuongThang((int)thang.Value, (int)nam.Value); grid.DataSource = _bangLuong.LayChiTiet((int)thang.Value, (int)nam.Value); return "Đã tính lương tháng."; }); };
            close.Click += delegate { Run(delegate { _bangLuong.ChotBangLuong((int)thang.Value, (int)nam.Value); grid.DataSource = _bangLuong.LayChiTiet((int)thang.Value, (int)nam.Value); return "Đã chốt bảng lương."; }); };
            view.Click += delegate { grid.DataSource = SafeLoad(delegate { return _bangLuong.LayChiTiet((int)thang.Value, (int)nam.Value); }); }; return FinishPage(page, layout, grid);
        }

        private static TabPage FinishPage(TabPage page, Control input, Control grid) { var split = new SplitContainer { Dock = DockStyle.Fill, Orientation = Orientation.Vertical, SplitterDistance = 560 }; split.Panel1.Controls.Add(input); split.Panel2.Controls.Add(grid); page.Controls.Add(split); return page; }
        private static TabPage NewPage(string text) { return new TabPage(text) { BackColor = Color.White }; }
        private static TableLayoutPanel NewLayout() { var p = new TableLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(16), ColumnCount = 2, RowCount = 20 }; p.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145)); p.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); return p; }
        private static DataGridView NewGrid() { return new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells, BackgroundColor = Color.White, RowHeadersVisible = false }; }
        private static FlowLayoutPanel NewActions() { return new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill, WrapContents = true }; }
        private static Button NewButton(string text) { return new Button { Text = text, AutoSize = true, Height = 34, BackColor = Color.FromArgb(25, 78, 121), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Margin = new Padding(4) }; }
        private static TextBox NewSmallText(string placeholder) { return new TextBox { Width = 110, Text = placeholder, Margin = new Padding(6, 9, 4, 4) }; }
        private static TextBox AddText(TableLayoutPanel p, string label, int row) { var c = new TextBox { Dock = DockStyle.Fill }; Add(p, label, c, row); return c; }
        private static DateTimePicker AddDate(TableLayoutPanel p, string label, int row, DateTime value) { var c = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy", Value = value }; Add(p, label, c, row); return c; }
        private static ComboBox AddCombo(TableLayoutPanel p, string label, int row, params string[] values) { var c = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList }; c.Items.AddRange(values); if (values.Length > 0) c.SelectedIndex = 0; Add(p, label, c, row); return c; }
        private static NumericUpDown AddMoney(TableLayoutPanel p, string label, int row, decimal value) { var c = AddNumber(p, label, row, value, 0, 1000000000, 0); c.ThousandsSeparator = true; return c; }
        private static NumericUpDown AddNumber(TableLayoutPanel p, string label, int row, decimal value, decimal min, decimal max, int decimals = 0) { var c = new NumericUpDown { Dock = DockStyle.Fill, Minimum = min, Maximum = max, Value = value, DecimalPlaces = decimals, ThousandsSeparator = false }; Add(p, label, c, row); return c; }
        private static void Add(TableLayoutPanel p, string label, Control c, int row) { p.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(3, 10, 3, 3) }, 0, row); p.Controls.Add(c, 1, row); }
        private static DateTime MondayOf(DateTime value) { int diff = ((int)value.DayOfWeek + 6) % 7; return value.Date.AddDays(-diff); }
        private static object SafeLoad(Func<object> loader) { try { return loader(); } catch { return null; } }
        private static void Run(Func<string> action) { try { MessageBox.Show(action(), "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information); } catch (Exception ex) { MessageBox.Show(ex.Message, "Không thể thực hiện", MessageBoxButtons.OK, MessageBoxIcon.Warning); } }
    }
}

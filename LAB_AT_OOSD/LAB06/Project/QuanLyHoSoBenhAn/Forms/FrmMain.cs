using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using QuanLyHoSoBenhAn.Models;
using QuanLyHoSoBenhAn.Services;

namespace QuanLyHoSoBenhAn.Forms
{
    public sealed class FrmMain : Form
    {
        private readonly PatientService _patients = new PatientService();
        private readonly ClinicalService _clinical = new ClinicalService();
        private TextBox _name, _phone, _address, _photo, _search, _appointmentRecord, _clinicalRecord, _symptoms, _diagnosis, _test, _instruction, _medicineId, _quantity, _dosage, _visitId, _paymentAmount;
        private DateTimePicker _birthDate, _appointmentTime;
        private NumericUpDown _annualFee;
        private DataGridView _grid;
        private ComboBox _paymentMethod;

        public FrmMain()
        {
            Text = "Quản lý hồ sơ bệnh án";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1100, 700);
            Width = 1250; Height = 780;
            BackColor = Color.White;
            Build();
        }

        private void Build()
        {
            var title = new Label { Dock = DockStyle.Top, Height = 54, Text = "QUẢN LÝ HỒ SƠ BỆNH ÁN", TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.White, BackColor = Color.FromArgb(28, 78, 121) };
            Controls.Add(title);
            var tabs = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10) };
            tabs.TabPages.Add(CreateRegistrationTab());
            tabs.TabPages.Add(CreateSearchTab());
            tabs.TabPages.Add(CreateAppointmentTab());
            tabs.TabPages.Add(CreateClinicalTab());
            tabs.TabPages.Add(CreatePaymentTab());
            Controls.Add(tabs);
        }

        private TabPage CreateRegistrationTab()
        {
            var page = new TabPage("Đăng ký - Hồ sơ"); var layout = NewLayout();
            _name = AddText(layout, "Họ tên", 0); _birthDate = AddDate(layout, "Ngày sinh", 1); _phone = AddText(layout, "Điện thoại", 2);
            _address = AddText(layout, "Địa chỉ", 3); _photo = AddText(layout, "Đường dẫn ảnh 3x4", 4); _annualFee = AddMoney(layout, "Lệ phí năm", 5, 200000);
            var button = NewButton("Đăng ký, thu lệ phí và cấp hồ sơ"); button.Click += RegisterPatient; layout.Controls.Add(button, 1, 6);
            AddNote(layout, "Nghiệp vụ chạy transaction sp_DangKyCapHoSo: nếu thu lệ phí hoặc cấp hồ sơ lỗi, toàn bộ đăng ký sẽ được rollback.", 7);
            page.Controls.Add(layout); return page;
        }

        private TabPage CreateSearchTab()
        {
            var page = new TabPage("Tra cứu lịch sử"); var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16) };
            var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 46 };
            top.Controls.Add(new Label { Text = "Mã hồ sơ / Họ tên / Điện thoại:", AutoSize = true, Padding = new Padding(0, 10, 5, 0) });
            _search = new TextBox { Width = 280 }; top.Controls.Add(_search);
            var button = NewButton("Tra cứu"); button.Click += SearchHistory; top.Controls.Add(button);
            _grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, AllowUserToAddRows = false, BackgroundColor = Color.White };
            panel.Controls.Add(_grid); panel.Controls.Add(top); page.Controls.Add(panel); return page;
        }

        private TabPage CreateAppointmentTab()
        {
            var page = new TabPage("Lịch khám định kỳ"); var layout = NewLayout();
            _appointmentRecord = AddText(layout, "Mã hồ sơ", 0); _appointmentTime = AddDate(layout, "Ngày giờ hẹn", 1); _appointmentTime.Format = DateTimePickerFormat.Custom; _appointmentTime.CustomFormat = "dd/MM/yyyy HH:mm"; _appointmentTime.ShowUpDown = true; _appointmentTime.Value = DateTime.Now.AddDays(7);
            var button = NewButton("Đặt lịch khám định kỳ miễn phí"); button.Click += CreateAppointment; layout.Controls.Add(button, 1, 2);
            AddNote(layout, "Chỉ chấp nhận hồ sơ đang sử dụng. Lịch định kỳ được lưu loại Định kỳ và không được trùng ngày giờ trong cùng hồ sơ.", 3);
            page.Controls.Add(layout); return page;
        }

        private TabPage CreateClinicalTab()
        {
            var page = new TabPage("Khám - Xét nghiệm - Toa"); var layout = NewLayout();
            _clinicalRecord = AddText(layout, "Mã hồ sơ", 0); var doctor = AddText(layout, "Mã bác sĩ", 1); doctor.Text = "1"; var department = AddText(layout, "Mã khoa", 2); department.Text = "1";
            _symptoms = AddText(layout, "Triệu chứng", 3); _diagnosis = AddText(layout, "Chẩn đoán", 4); _test = AddText(layout, "Chỉ định xét nghiệm (có thể trống)", 5);
            _medicineId = AddText(layout, "Mã thuốc", 6); _medicineId.Text = "1"; _quantity = AddText(layout, "Số lượng", 7); _quantity.Text = "10";
            _dosage = AddText(layout, "Liều dùng", 8); _dosage.Text = "1 viên/lần"; _instruction = AddText(layout, "Lời dặn / cách dùng", 9);
            var button = NewButton("Lập phiếu khám, xét nghiệm và toa thuốc"); button.Click += (s, e) => CreateClinical(_clinicalRecord.Text, doctor.Text, department.Text); layout.Controls.Add(button, 1, 10);
            AddNote(layout, "Thao tác gọi sp_LapPhieuKhamVaToa và sử dụng table-valued parameter cho chi tiết toa thuốc.", 11);
            page.Controls.Add(layout); return page;
        }

        private TabPage CreatePaymentTab()
        {
            var page = new TabPage("Thanh toán - Hoàn tất"); var layout = NewLayout();
            _visitId = AddText(layout, "Mã phiếu khám", 0); _paymentAmount = AddText(layout, "Số tiền viện phí", 1); _paymentMethod = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList }; _paymentMethod.Items.AddRange(new object[] { "Tiền mặt", "Chuyển khoản", "Thẻ" }); _paymentMethod.SelectedIndex = 0; AddControl(layout, "Phương thức", _paymentMethod, 2);
            var button = NewButton("Thanh toán và hoàn tất điều trị"); button.Click += Pay; layout.Controls.Add(button, 1, 3);
            AddNote(layout, "Chỉ phiếu khám đang điều trị mới thanh toán được. Sau khi thanh toán, hệ thống cập nhật trạng thái Hoàn tất.", 4);
            page.Controls.Add(layout); return page;
        }

        private void RegisterPatient(object sender, EventArgs e)
        {
            Run(() => { var code = _patients.RegisterAndCreateRecord(new PatientRegistration { FullName = _name.Text, DateOfBirth = _birthDate.Value, Phone = _phone.Text, Address = _address.Text, PhotoPath = _photo.Text, AnnualFee = _annualFee.Value, Year = DateTime.Today.Year }); MessageBox.Show("Đã cấp hồ sơ: " + code, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information); });
        }
        private void SearchHistory(object sender, EventArgs e) { Run(() => _grid.DataSource = _patients.SearchHistory(_search.Text)); }
        private void CreateAppointment(object sender, EventArgs e) { Run(() => { _patients.CreateQuarterlyAppointment(_appointmentRecord.Text, _appointmentTime.Value); MessageBox.Show("Đã đặt lịch hẹn.", "Thành công"); }); }
        private void CreateClinical(string record, string doctor, string department) { Run(() => { var visit = _clinical.CreateExaminationAndPrescription(record, int.Parse(doctor), int.Parse(department), _symptoms.Text, _diagnosis.Text, _test.Text, _instruction.Text, int.Parse(_medicineId.Text), int.Parse(_quantity.Text), _dosage.Text); _visitId.Text = visit.ToString(CultureInfo.InvariantCulture); MessageBox.Show("Đã lập phiếu khám và toa thuốc. Mã phiếu khám: " + visit, "Thành công"); }); }
        private void Pay(object sender, EventArgs e) { Run(() => { _clinical.PayAndComplete(int.Parse(_visitId.Text), decimal.Parse(_paymentAmount.Text, CultureInfo.CurrentCulture), _paymentMethod.Text); MessageBox.Show("Đã thanh toán và hoàn tất điều trị.", "Thành công"); }); }
        private void Run(Action action) { try { action(); } catch (Exception ex) { MessageBox.Show(ex.Message, "Không thể thực hiện", MessageBoxButtons.OK, MessageBoxIcon.Warning); } }

        private static TableLayoutPanel NewLayout() { return new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Padding = new Padding(28), AutoScroll = true, ColumnStyles = { new ColumnStyle(SizeType.Absolute, 240), new ColumnStyle(SizeType.Percent, 100) } }; }
        private static TextBox AddText(TableLayoutPanel layout, string label, int row) { var box = new TextBox { Dock = DockStyle.Fill }; AddControl(layout, label, box, row); return box; }
        private static DateTimePicker AddDate(TableLayoutPanel layout, string label, int row) { var value = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, MaxDate = DateTime.Today }; AddControl(layout, label, value, row); return value; }
        private static NumericUpDown AddMoney(TableLayoutPanel layout, string label, int row, decimal value) { var control = new NumericUpDown { Dock = DockStyle.Fill, Minimum = 1, Maximum = 10000000, Increment = 50000, Value = value, ThousandsSeparator = true }; AddControl(layout, label, control, row); return control; }
        private static void AddControl(TableLayoutPanel layout, string label, Control control, int row) { layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); layout.Controls.Add(new Label { Text = label + ":", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(0, 7, 8, 7) }, 0, row); layout.Controls.Add(control, 1, row); }
        private static Button NewButton(string text) { return new Button { Text = text, AutoSize = true, Height = 34, BackColor = Color.FromArgb(28, 78, 121), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Padding = new Padding(8, 0, 8, 0) }; }
        private static void AddNote(TableLayoutPanel layout, string text, int row) { var note = new Label { Text = text, AutoSize = true, MaximumSize = new Size(760, 0), ForeColor = Color.DimGray, Padding = new Padding(0, 12, 0, 0) }; layout.SetColumnSpan(note, 2); layout.Controls.Add(note, 0, row); }
    }
}

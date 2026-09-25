using System;
using System.Data;
using System.Windows.Forms;
using QuanLyKhachSan.Services;

namespace QuanLyKhachSan.Forms
{
    public partial class FrmThongKe : Form
    {
        private readonly NghiepVuService service = new NghiepVuService();
        public FrmThongKe() { InitializeComponent(); Load += (s, e) => Tai(); }
        protected override void OnLoad(EventArgs e) { base.OnLoad(e); UiTheme.Find<Button>(this, "btnThongKe").Click += (s, a) => Tai(); }
        private void Tai()
        {
            DateTime tu = UiTheme.Find<DateTimePicker>(this, "dtTu").Value.Date, den = UiTheme.Find<DateTimePicker>(this, "dtDen").Value.Date; if (den < tu) { MessageBox.Show("Đến ngày không được trước từ ngày."); return; }
            var summary = UiTheme.Find<DataGridView>(this, "dgvTongHop"); summary.Columns.Clear(); summary.AutoGenerateColumns = true; summary.DataSource = service.TongHop(tu, den);
            var detail = UiTheme.Find<DataGridView>(this, "dgvDichVu"); detail.Columns.Clear(); detail.AutoGenerateColumns = true; detail.DataSource = service.ThongKeDichVu(tu, den);
            DataRow row = service.ChiSo(tu, den).Rows[0]; SetCard("Phiếu đặt", Convert.ToString(row["SoPhieuDat"])); SetCard("Khách đang ở", Convert.ToString(row["DangO"])); SetCard("Doanh thu", Convert.ToDecimal(row["DoanhThu"]).ToString("N0") + " đ"); SetCard("Tiền đền bù", Convert.ToDecimal(row["DenBu"]).ToString("N0") + " đ");
        }
        private void SetCard(string name, string value) { foreach (Panel p in UiTheme.FindAll<Panel>(this)) if (Convert.ToString(p.Tag) == name) { var label = UiTheme.Find<Label>(p, "lblCardValue"); if (label != null) label.Text = value; } }
    }
}

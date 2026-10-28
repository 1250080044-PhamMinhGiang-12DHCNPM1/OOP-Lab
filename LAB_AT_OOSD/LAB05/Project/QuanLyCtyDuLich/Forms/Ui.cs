using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyCongTyDuLich.Services;

namespace QuanLyCongTyDuLich.Forms
{
    internal static class Ui
    {
        public static TextBox Text(string placeholder, int width = 130) { var t=new TextBox { Width=width, Margin=new Padding(4), Tag=placeholder }; t.AccessibleName=placeholder; return t; }
        public static NumericUpDown Number(decimal value=0, decimal max=999999999) { return new NumericUpDown { Width=105, Maximum=max, DecimalPlaces=0, Value=value, Margin=new Padding(4) }; }
        public static Button Button(string text, EventHandler click) { var b=new Button { Text=text, AutoSize=true, BackColor=Color.FromArgb(31,78,120), ForeColor=Color.White, FlatStyle=FlatStyle.Flat, Margin=new Padding(4) }; b.Click+=click; return b; }
        public static DataGridView Grid() { return new DataGridView { Dock=DockStyle.Fill, ReadOnly=true, AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill, AllowUserToAddRows=false, BackgroundColor=Color.White }; }
        public static FlowLayoutPanel Top() { return new FlowLayoutPanel { Dock=DockStyle.Top, AutoSize=true, Padding=new Padding(8), BackColor=Color.FromArgb(234,243,248), WrapContents=true }; }
        public static void Show(KetQuaXuLy k) { MessageBox.Show(k.ThongBao, k.ThanhCong ? "Thành công" : "Kiểm tra dữ liệu", MessageBoxButtons.OK, k.ThanhCong ? MessageBoxIcon.Information : MessageBoxIcon.Warning); }
        public static string Cell(DataGridView grid, string col) { return grid.CurrentRow == null ? "" : Convert.ToString(grid.CurrentRow.Cells[col].Value); }
    }
}

using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuanLyKhachSan.Forms
{
    internal static class UiTheme
    {
        public static readonly Color Navy = Color.FromArgb(18, 48, 86);
        public static readonly Color Blue = Color.FromArgb(34, 110, 210);
        public static readonly Color PaleBlue = Color.FromArgb(232, 242, 255);
        public static readonly Color Background = Color.FromArgb(245, 247, 251);
        public static readonly Color Border = Color.FromArgb(218, 224, 232);
        public static readonly Color Text = Color.FromArgb(42, 49, 61);

        public static void StyleForm(Form form, string title)
        {
            form.Text = title; form.BackColor = Background; form.Font = new Font("Segoe UI", 10F);
            form.StartPosition = FormStartPosition.CenterScreen; form.MinimumSize = new Size(1000, 650);
        }

        public static Label Heading(string text, float size)
        {
            return new Label { Text = text, AutoSize = true, ForeColor = Navy, Font = new Font("Segoe UI Semibold", size) };
        }

        public static Button Button(string text, bool primary)
        {
            var b = new Button { Text = text, Height = 38, Width = 112, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand,
                BackColor = primary ? Blue : Color.White, ForeColor = primary ? Color.White : Navy, Margin = new Padding(5) };
            b.FlatAppearance.BorderColor = primary ? Blue : Border; return b;
        }

        public static DataGridView Grid(params string[] columns)
        {
            var g = new DataGridView { Dock = DockStyle.Fill, BackgroundColor = Color.White, BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, AllowUserToAddRows = false, ReadOnly = true,
                RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            g.EnableHeadersVisualStyles = false; g.ColumnHeadersDefaultCellStyle.BackColor = Navy; g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F); g.ColumnHeadersHeight = 38; g.RowTemplate.Height = 34;
            g.DefaultCellStyle.SelectionBackColor = PaleBlue; g.DefaultCellStyle.SelectionForeColor = Text;
            foreach (string column in columns) g.Columns.Add(column.Replace(" ", ""), column);
            return g;
        }

        public static TextBox Input(string name) { return new TextBox { Name = name, Width = 180, Height = 30 }; }
        public static ComboBox Combo(string name, params string[] items)
        {
            var c = new ComboBox { Name = name, Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            c.Items.AddRange(items); if (items.Length > 0) c.SelectedIndex = 0; return c;
        }

        public static FlowLayoutPanel Field(string label, Control control, int width = 200)
        {
            var p = new FlowLayoutPanel { Width = width, Height = 68, FlowDirection = FlowDirection.TopDown, WrapContents = false, Margin = new Padding(7) };
            p.Controls.Add(new Label { Text = label, AutoSize = true, ForeColor = Text, Font = new Font("Segoe UI Semibold", 9F) });
            control.Width = width - 12; p.Controls.Add(control); return p;
        }

        public static Panel Card(string caption, string value, Color accent)
        {
            var p = new Panel { Name = "card", Tag = caption, BackColor = Color.White, Width = 190, Height = 105, Margin = new Padding(8) };
            p.Controls.Add(new Panel { Dock = DockStyle.Left, Width = 5, BackColor = accent });
            p.Controls.Add(new Label { Text = caption, AutoSize = true, ForeColor = Color.DimGray, Location = new Point(20, 18) });
            p.Controls.Add(new Label { Name = "lblCardValue", Text = value, AutoSize = true, ForeColor = Navy, Font = new Font("Segoe UI Semibold", 24F), Location = new Point(18, 45) });
            return p;
        }

        public static TabPage CrudPage(string title, string[] columns, string[] fields)
        {
            var page = new TabPage(title) { Name = "tabData", BackColor = Background, Padding = new Padding(12) };
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3, ColumnCount = 1 };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52)); layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 145));
            var search = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(3, 6, 3, 3) };
            search.Controls.Add(new Label { Text = "Tìm kiếm", AutoSize = true, Margin = new Padding(0, 10, 8, 0) }); search.Controls.Add(Input("txtTim")); var refreshTop = Button("Làm mới", false); refreshTop.Name = "btnTaiLai"; search.Controls.Add(refreshTop);
            layout.Controls.Add(search, 0, 0); var grid = Grid(columns); grid.Name = "dgvData"; layout.Controls.Add(grid, 0, 1);
            var editor = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White, Padding = new Padding(8) };
            for (int i = 0; i < fields.Length; i++) editor.Controls.Add(Field(fields[i], Input("txtField" + i)));
            var add = Button("Thêm", true); add.Name = "btnThem"; var edit = Button("Sửa", false); edit.Name = "btnSua"; var delete = Button("Xóa", false); delete.Name = "btnXoa"; var clear = Button("Làm mới", false); clear.Name = "btnLamMoi";
            editor.Controls.Add(add); editor.Controls.Add(edit); editor.Controls.Add(delete); editor.Controls.Add(clear);
            layout.Controls.Add(editor, 0, 2); page.Controls.Add(layout); return page;
        }

        public static T Find<T>(Control root, string name) where T : Control
        {
            foreach (Control control in root.Controls) { if (control is T && control.Name == name) return (T)control; T child = Find<T>(control, name); if (child != null) return child; }
            return null;
        }

        public static List<T> FindAll<T>(Control root) where T : Control
        {
            var result = new List<T>(); FindAll(root, result); return result;
        }
        private static void FindAll<T>(Control root, List<T> result) where T : Control
        {
            foreach (Control control in root.Controls) { if (control is T) result.Add((T)control); FindAll(control, result); }
        }
    }
}

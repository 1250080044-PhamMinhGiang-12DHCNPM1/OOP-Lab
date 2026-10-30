using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using ChoThueVanPhongCaoOc.Data;

namespace ChoThueVanPhongCaoOc.Forms
{
    internal enum FieldKind { Text, Integer, Decimal, Date }

    internal sealed class FieldSpec
    {
        internal string Label, Parameter;
        internal FieldKind Kind;
        internal FieldSpec(string label, string parameter, FieldKind kind = FieldKind.Text) { Label = label; Parameter = parameter; Kind = kind; }
    }

    internal sealed class CrudPanel : UserControl
    {
        private readonly string _selectSql, _insertSql;
        private readonly DataGridView _grid = new DataGridView();
        private readonly List<Tuple<FieldSpec, TextBox>> _fields = new List<Tuple<FieldSpec, TextBox>>();

        internal CrudPanel(string selectSql, string insertSql, params FieldSpec[] fields)
        {
            _selectSql = selectSql; _insertSql = insertSql; Dock = DockStyle.Fill;
            var input = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 72, AutoScroll = true, Padding = new Padding(8) };
            foreach (var f in fields)
            {
                var box = new TextBox { Width = 125 };
                var group = new FlowLayoutPanel { Width = 150, Height = 52, FlowDirection = FlowDirection.TopDown };
                group.Controls.Add(new Label { Text = f.Label, AutoSize = true }); group.Controls.Add(box); input.Controls.Add(group);
                _fields.Add(Tuple.Create(f, box));
            }
            var save = new Button { Text = "Thêm", Width = 85, Height = 30, Margin = new Padding(8, 18, 4, 4) };
            var reload = new Button { Text = "Tải lại", Width = 85, Height = 30, Margin = new Padding(4, 18, 4, 4) };
            save.Click += (s, e) => Save(); reload.Click += (s, e) => LoadData(); input.Controls.Add(save); input.Controls.Add(reload);
            _grid.Dock = DockStyle.Fill; _grid.ReadOnly = true; _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; _grid.BackgroundColor = Color.White;
            Controls.Add(_grid); Controls.Add(input); Load += (s, e) => LoadData();
        }

        private void LoadData()
        {
            try { _grid.DataSource = Db.Query(_selectSql); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi tải dữ liệu"); }
        }

        private void Save()
        {
            try
            {
                var ps = new List<SqlParameter>();
                foreach (var item in _fields)
                {
                    object value;
                    switch (item.Item1.Kind)
                    {
                        case FieldKind.Integer: value = int.Parse(item.Item2.Text, CultureInfo.InvariantCulture); break;
                        case FieldKind.Decimal: value = decimal.Parse(item.Item2.Text, CultureInfo.CurrentCulture); break;
                        case FieldKind.Date: value = DateTime.Parse(item.Item2.Text, CultureInfo.CurrentCulture).Date; break;
                        default: value = string.IsNullOrWhiteSpace(item.Item2.Text) ? (object)DBNull.Value : item.Item2.Text.Trim(); break;
                    }
                    ps.Add(new SqlParameter(item.Item1.Parameter, value));
                }
                Db.Execute(_insertSql, ps.ToArray());
                foreach (var item in _fields) item.Item2.Clear();
                LoadData();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Không thể lưu"); }
        }
    }
}

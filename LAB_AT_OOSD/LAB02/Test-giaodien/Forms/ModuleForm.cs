using System.Drawing;
using System.Windows.Forms;

namespace Test_giaodien.Forms
{
    public class ModuleForm : Form
    {
        private readonly Panel contentPanel;

        protected ModuleForm(string title, string subtitle)
        {
            Text = title;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(740, 430);
            Font = new Font("Segoe UI", 10F);
            BackColor = Color.White;

            Label titleLabel = new Label
            {
                Text = title,
                Dock = DockStyle.Top,
                Height = 70,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                BackColor = Color.FromArgb(31, 78, 121),
                ForeColor = Color.White
            };
            Label subtitleLabel = new Label
            {
                Text = subtitle,
                Dock = DockStyle.Top,
                Height = 46,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.DimGray
            };
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(32, 20, 32, 72)
            };
            Button closeButton = new Button
            {
                Text = "Đóng",
                Size = new Size(110, 36),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
                Location = new Point(ClientSize.Width - 132, ClientSize.Height - 56),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += (s, e) => Close();

            Controls.Add(contentPanel);
            Controls.Add(closeButton);
            Controls.Add(subtitleLabel);
            Controls.Add(titleLabel);
            closeButton.BringToFront();
        }

        protected void AddPlaceholder(string message)
        {
            Label placeholder = new Label
            {
                Text = message,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(90, 90, 90),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 11F, FontStyle.Italic)
            };
            contentPanel.Controls.Add(placeholder);
        }
    }
}

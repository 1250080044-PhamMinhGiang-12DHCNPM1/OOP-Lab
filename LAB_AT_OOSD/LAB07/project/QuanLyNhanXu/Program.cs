using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using QuanLyNhanXu.Forms;

namespace QuanLyNhanXu
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args != null && args.Length == 2 && args[0] == "--capture")
            {
                CaptureScreens(args[1]);
                return;
            }
            Application.Run(new FrmMain());
        }

        private static void CaptureScreens(string folder)
        {
            Directory.CreateDirectory(folder);
            using (var form = new FrmMain())
            {
                form.Show();
                Application.DoEvents();
                TabControl tabs = null;
                foreach (Control control in form.Controls) if (control is TabControl) tabs = (TabControl)control;
                if (tabs == null) throw new InvalidOperationException("Không tìm thấy TabControl.");
                for (int i = 0; i < tabs.TabCount; i++)
                {
                    tabs.SelectedIndex = i;
                    Application.DoEvents();
                    using (var bitmap = new Bitmap(form.ClientSize.Width, form.ClientSize.Height))
                    {
                        form.DrawToBitmap(bitmap, new Rectangle(Point.Empty, form.ClientSize));
                        bitmap.Save(Path.Combine(folder, string.Format("form-{0:00}.png", i + 1)), ImageFormat.Png);
                    }
                }
                form.Close();
            }
        }
    }
}

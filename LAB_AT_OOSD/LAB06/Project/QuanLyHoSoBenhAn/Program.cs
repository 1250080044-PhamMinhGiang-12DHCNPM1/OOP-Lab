using System;
using System.Windows.Forms;
using QuanLyHoSoBenhAn.Forms;

namespace QuanLyHoSoBenhAn
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
    }
}

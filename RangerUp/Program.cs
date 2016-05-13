using System;
using System.Windows.Forms;

namespace RangerUp
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var form1 = new Form1())
            {
                form1.Show();
                form1.GameLoop();
            }
        }
    }
}
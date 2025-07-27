using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Speech.Synthesis;
using System.Drawing;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Media;
using System.Reflection;
using System.Collections.Generic;

namespace Injector
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_HIDE = 0x0;

        [STAThread]
        static void Main(string[] args)
        {
            IntPtr handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppForm form = new AppForm();
            Application.Run(form);
        }
    }
}


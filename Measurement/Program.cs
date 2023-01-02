using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LZ.CNC.Measurement.Forms
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                bool flg = false;
                Mutex mutex = new Mutex(true, "1914", out flg);            
                if (!flg)
                {
                    MessageBox.Show("已经启动了一个相同的程序，不可再次启动！");
                    Environment.Exit(1);
                    return;
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Application.Exit();
            }
          
        }
    }
}

using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace LZ.CNC.Measurement.Core.Logs
{
    public class LogManager
    {
        private static object _LockObj;
        public static String FilePath
        {
            get
            {
                return Path.Combine(Application.StartupPath, string.Format("Logs\\{0}.txt", DateTime.Today.ToString("yyyy_MM_dd")));
            }
        }

        static LogManager()
        {
            _LockObj = new object();
            string dir = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public static void WriteLine(string msg)
        {
            object lockObj;
            Monitor.Enter(lockObj = _LockObj);
            try
            {
                FileStream fs;
                if (File.Exists(FilePath))
                {
                    fs = File.Open(FilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                }
                else
                {
                    fs = File.Open(FilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                }
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(string.Format("{0} : {1}", DateTime.Now.ToString("HH:mm:ss"), msg));
                sw.Close();
                fs.Close();
            }
            finally
            {
                Monitor.Exit(lockObj);
            }
        }
    }
}

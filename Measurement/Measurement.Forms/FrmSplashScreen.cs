using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LZ.CNC.Measurement.Forms
{
    public partial class FrmSplashScreen : Form
    {
        public FrmSplashScreen()
        {
           
            InitializeComponent();
            string showInfo = "";
            Bitmap bitmap = null;
            showInfo = "Loading...";
            bitmap = new Bitmap(Application.StartupPath + "\\Startup1.jpg");
            ClientSize = bitmap.Size;
            using (Font font = new Font("Consoles", 40))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.DrawString(showInfo, font, Brushes.Black, 1250, 850);
                }
            }
            BackgroundImage = bitmap;
        }

        private void FrmSplashScreen_Load(object sender, EventArgs e)
        {

        }
    }
}

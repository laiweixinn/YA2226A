using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace LZ.CNC.Measurement.Core
{
    public partial class FrmConfirmAlarm : Form
    {
        public FrmConfirmAlarm(string info = null)
        {
            InitializeComponent();
            if(info!=null)
            {
                string inf = info + "报警";
                lblInfo.Text = inf;
                MeasurementContext.OutputError(inf);
            }
        }

        MeasurementWorker _worker = MeasurementContext.Worker;
        private void btnOk_Click(object sender, EventArgs e)
        {
            _worker.CloseBuzzer();
            _worker.CloseRedLight();
            _worker.OpenGreenLight();
            _worker.LockSafeDoor();
            _worker.FlagAlarm = false;
            if (MeasurementContext.Worker.workstatus != WorkStatuses.Pausing)
            {
                _worker.IsAutoRun = true;
            }
            this.DialogResult = DialogResult.OK;
        }

        private void btn_closebuzzer_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            _worker.CloseBuzzer();
            this.BackColor = Color.Goldenrod;
        }

        private void FrmConfirmAlarm_Load(object sender, EventArgs e)
        {
            _worker.IsAutoRun = false;
            _worker.FlagAlarm = true;
            _worker.AlarmWork();
            timer1.Start();    
            _worker.UnlockSafeDoor();
        }


        bool showforecolor = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.TopLevel = true;
            this.TopMost = true;
            this.Focus();
            if (showforecolor)
            {
                this.BackColor = Color.Goldenrod;
                _worker.OpenBuzzer();
            }
            else
            {
                this.BackColor = Color.White;
                _worker.CloseBuzzer();
            }
            showforecolor = !showforecolor;
        }

        #region 窗体移动
        private Point mouseOff;//鼠标移动位置变量
        private bool leftFlag;//标签是否为左键
        private void frm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y);//得到变量的值
                leftFlag = true;                 //点击左键按下时标注为true；
            }
        }
        private void frm_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);//设置移动后的位置
                Location = mouseSet;
            }
        }
        private void frm_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false; //释放鼠标后标注为false；
            }
        }
        #endregion










    }
}

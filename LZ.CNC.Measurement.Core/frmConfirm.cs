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
    public partial class frmConfirm : Form
    {

        public MeasurementConfig.ConfigIOOut IOOut1;
        public MeasurementConfig.ConfigIOOut IOOut2;
        public MeasurementConfig.ConfigIOOut IOOut3;
        public MeasurementConfig.ConfigIOOut IOOut4;
        public MeasurementConfig.ConfigIOOutEx IOOutEx1;
        public MeasurementConfig.ConfigIOOutEx IOOutEx2;
        public MeasurementConfig.ConfigIOOutEx IOOutEx3;
        public MeasurementConfig.ConfigIOOutEx IOOutEx4;

        public frmConfirm(string info = null, bool showcancel = false, bool showAbort = false)
        {
            InitializeComponent();
            if (info != null)
            {
                lblInfo.Text = info;
                MeasurementContext.OutputMessage(info);
            }

            _worker.AlarmWork();
            btnCancel.Visible = showcancel;
            btnAbort.Visible = showAbort;
            this.TopMost = true;
            this.TopLevel = true;
        }

        MeasurementWorker _worker = MeasurementContext.Worker;

        private void btnOk_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            if (_worker.IsGateSafe())
            {
                _worker.FlagConfirm = false;
                _worker.CloseBuzzer();
                _worker.CloseRedLight();
                _worker.OpenGreenLight();
                _worker.FlagConfirm = false;
                _worker.b_AlarmFlag = true;

                if (MeasurementContext.Worker.workstatus != WorkStatuses.Pausing)
                {
                    _worker.IsAutoRun = true;
                }
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                timer1.Start();
                _worker.AlarmWork();
                lblInfo.AppendText("\r\n 门禁触发");
            }
            _worker.LockSafeDoor();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void frmConfirm_Shown(object sender, EventArgs e)
        {
            this.TopMost = true;
            btnOk.Focus();
            timer1.Enabled = true;
        }

        bool showforecolor = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.TopLevel = true;
            this.TopMost = true;
             
        }

        private void frmConfirm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            if (IOOut1 != null)
            {
                _worker.SetIOOut(IOOut1, true);
                Thread.Sleep(200);
                _worker.SetIOOut(IOOut1, false);
            }

            if (IOOut2 != null)
            {
                _worker.SetIOOut(IOOut2, false);
            }

            if (IOOut3 != null)
            {
                _worker.SetIOOut(IOOut3, false);

            }

            if (IOOut4 != null)
            {
                _worker.SetIOOut(IOOut4, true);
                Thread.Sleep(200);
                _worker.SetIOOut(IOOut4, false);
            }

            if (IOOutEx1 != null)
            {
                _worker.CanSetIOOut(IOOutEx1, true);
                Thread.Sleep(200);
                _worker.SetIOOut(IOOutEx1, false);
            }

            if (IOOutEx2 != null)
            {
                _worker.CanSetIOOut(IOOutEx2, false);
            }

            if (IOOutEx3 != null)
            {
                _worker.CanSetIOOut(IOOutEx3, false);
            }

            if (IOOutEx4 != null)
            {
                _worker.CanSetIOOut(IOOutEx4, true);
                Thread.Sleep(200);
                _worker.CanSetIOOut(IOOutEx4, false);
            }
        }

        private void btn_closebuzzer_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            _worker.CloseBuzzer();
            this.BackColor = Color.Goldenrod;

        }

        private void frmConfirm_Load(object sender, EventArgs e)
        {
            _worker.IsAutoRun = false;
            _worker.FlagConfirm = true;
            _worker.AlarmWork();
            _worker.UnlockSafeDoor();
            _worker.b_AlarmFlag = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _worker.UnlockSafeDoor();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (MeasurementContext.Worker.CanGetIOInStatus(MeasurementContext.Config.StartBtnIOInEx))//start默认为确认
            {
               btnOk_Click(null, null);
            }

            if (btnCancel.Visible && MeasurementContext.Worker.CanGetIOInStatus(MeasurementContext.Config.ResetbtnIOInEx))//复位默认取消
            {
                btnCancel_Click(null, null);
            }
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

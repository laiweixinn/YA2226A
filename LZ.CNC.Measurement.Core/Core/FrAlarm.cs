using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LZ.CNC.Measurement.Core
{
    public partial class FrAlarm : Form
    {
        public FrAlarm()
        {
            InitializeComponent();
        }

        MeasurementWorker _worker = MeasurementContext.Worker;

        private void FrAlarm_Load(object sender, EventArgs e)
        {
            MeasurementContext.OutputError(lblmsg.Text);
            _worker.IsAutoRun = false;
            _worker.b_AlarmFlag = false;
            _worker.AlarmWork();          
            _worker.UnlockSafeDoor();
            this.BackColor = Color.Goldenrod;
            _worker.OpenBuzzer();
            this.TopLevel = true;
            this.TopMost = true;
            timer1.Start();
            timer2.Start();
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
            _worker.EstopMachine();
            this.Close();
        }

        private void btn_continue_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            if (_worker.IsGateSafe())
            {
                _worker.CloseBuzzer();
                _worker.CloseRedLight();
                _worker.OpenGreenLight();
                if (MeasurementContext.Worker. workstatus!= WorkStatuses.Pausing)
                {
                    _worker.IsAutoRun = true;
                }              
                _worker.b_AlarmFlag = true;               
            }
            else
            {
                timer1.Start();
                _worker.AlarmWork();
                lblmsg.AppendText("\r\n 门禁触发");
            }
            this.Close();
        }

        private void btn_closebuzzer_Click(object sender, EventArgs e)
        {
            _worker.CloseBuzzer();
        }

        bool showforecolor = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.TopLevel = true;
            this.TopMost = true;
            //if (showforecolor)
            //{
            //    this.BackColor = Color.Goldenrod;
            //    _worker.OpenBuzzer();
            //}
            //else
            //{
            //    this.BackColor = Color.White;
            //    _worker.CloseBuzzer();
            //}
            //showforecolor = !showforecolor;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (MeasurementContext.Worker.CanGetIOInStatus(MeasurementContext.Config.StartBtnIOInEx))//确认
            {
                btn_continue_Click(null, null);
            }

            //if (MeasurementContext.Worker.CanGetIOInStatus(MeasurementContext.Config.ResetbtnIOInEx))//取消
            //{
            //    btn_stop_Click(null, null);
            //}
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using LZ.CNC.Measurement.Core;
using System.Threading;

namespace LZ.CNC
{
    public partial class FrPause : Form
    {
        public FrPause()
        {
            InitializeComponent();
        }

        MeasurementWorker _worker = MeasurementContext.Worker;

        private void timer1_Tick(object sender, EventArgs e)
        {
             
            //if(label2.BackColor ==System.Drawing.Color.Red)
            //{
            //    label2.BackColor = SystemColors.AppWorkspace;
            //}
            //else
            //{
            //    label2.BackColor = System.Drawing.Color.Red;
            //}
        }

        private void FrPause_Load(object sender, EventArgs e)
        {
            _worker.PauseMachine();
            timer1.Start();
        }

        private void FrPause_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Stop();
            _worker.CloseBuzzer();
        }

        private void btn_stop_Click(object sender, EventArgs e)
        {
       
            _worker.IsAutoRun = true;
            _worker.FeedStop = true;
            _worker.Tear1Stop = true;
            _worker.Tear2Stop = true;
            _worker.Tear3Stop = true;
            _worker.Bend1Stop = true;
            _worker.Bend2Stop = true;
            _worker.Bend3Stop = true;
            _worker.TransferStop = true;
            _worker.DischargeStop = true;
            _worker.ManualStop = true;
            _worker.StopworkStausChanged();
            this.Close();
        }

        private void btn_continue_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            _worker.CloseBuzzer();
            _worker.ContiMachine();            
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            _worker.ContiMachine();
            Thread.Sleep(200);
            _worker.StopMachine();
            this.Close();        
        }

        bool showforecolor = false;
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            this.TopLevel = true;
            this.TopMost = true;
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

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            _worker.CloseBuzzer();
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

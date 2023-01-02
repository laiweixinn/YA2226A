using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DY.Core.Forms;
using LZ.CNC.UserLevel;
using DY.Core.Util;
using LZ.CNC.Measurement.Core;
using LZ.CNC.Measurement.Core.Motions;
using LZ.CNC.Measurement.Core.Events;
using LZ.CNC.Measurement.Forms.Controls;
using System.Threading;

namespace LZ.CNC.Measurement.Forms
{
    public partial class FrMain : Form
    {

        private FrmIO _FrIO;
        private FrmSet _FrSet;
        private FrDebug _FrDebug;
        private TabForm[] _TabForms;
        private MeasurementWorker Worker = MeasurementContext.Worker;
        public FrMain()
        {
            InitializeComponent();
            Task.Run(()=> MeasurementContext.Init());
            Task.WaitAll();
            _FrIO = new FrmIO();
            _FrSet = new FrmSet();
            _FrDebug = new FrDebug();
            _TabForms = new TabForm[] { _FrIO, _FrSet, _FrDebug };
          
        }



        private void buttonEx1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrMain_Load(object sender, EventArgs e)
        {
            timer1.Start();
            Worker.ConnectCard();
        }

                 



        private void rbtn_run_Click(object sender, EventArgs e)
        {
            TabForm[] forms = _TabForms;

            
            switch (((RadioButton)sender).Name)
            {
                case "rbt_io":
                    //form_sel(((RadioButton)sender).Name);
                    FormUtil.AddFormToControl(panel2, _FrIO);
                    break;
                case "rbt_debug":
                    FormUtil.AddFormToControl(panel2, _FrDebug);

                    break;
                case "rbt_set":
                    FormUtil.AddFormToControl(panel2, _FrSet);
                    break;
                default:
                    
                    break;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                //if (MessageBox.Show("提示","是否退出系统？"))
                //{
                    Worker.EndMotion();
                    Worker.StopSlowly();

                    this.Close();
                //}
            }
        }

        private DateTime _StartTime = DateTime.Now;
        private void timer1_Tick(object sender, EventArgs e)
        {
            label_date.Text = DateTime.Today.ToString("yyyy年MM月dd日") + DateTime.Now.ToString("HH时mm分ss秒");
            TimeSpan ts = DateTime.Now - _StartTime;
            label_runtime.Text = string.Format("{0} 天 {1} 时 {2} 分 {3} 秒", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
        }
    }
}

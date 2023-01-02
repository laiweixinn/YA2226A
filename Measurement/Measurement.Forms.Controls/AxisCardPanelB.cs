using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LZ.CNC.Measurement.Core.Motions;
using LZ.CNC.Measurement.Core;


namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class AxisCardPanelB : PageUC.BasePageUC
    {
        public AxisCardPanelB()
        {
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.ExStyle |= 0x02000000; // Turn off WS_CLIPCHILDREN 
                return parms;
            }
        }

        //protected override void WndProc(ref Message m)
        //{
        //    if (m.Msg == 0x0014) 
        //        return;
        //    base.WndProc(ref m);
        //}

        public override void Init()
        {
            MeasurementWorker worker = MeasurementContext.Worker;
            axisMovePanel1.Axis = worker.MotionB.AxisX;
            axisMovePanel2.Axis = worker.MotionB.AxisY;
            axisMovePanel3.Axis = worker.MotionB.AxisZ;
            axisMovePanel4.Axis = worker.MotionB.AxisU;
            axisMovePanel5.Axis = worker.MotionB.AxisV;
            axisMovePanel6.Axis = worker.MotionB.AxisW;
            axisMovePanel7.Axis = worker.MotionB.AxisA;
            axisMovePanel8.Axis = worker.MotionB.AxisB;
            axisMovePanel9.Axis = worker.MotionB.AxisC;
            axisMovePanel10.Axis = worker.MotionB.AxisD;

        }


        public void Init(MeasurementAxis[] Axises)
        {
            axisMovePanel1.Axis = Axises[0];
            axisMovePanel2.Axis = Axises[1];
            axisMovePanel3.Axis = Axises[2];
            axisMovePanel4.Axis = Axises[3];
            axisMovePanel5.Axis = Axises[4];
            axisMovePanel6.Axis = Axises[5];
            axisMovePanel7.Axis = Axises[6];
            axisMovePanel8.Axis = Axises[7];
            axisMovePanel9.Axis = Axises[8];
            axisMovePanel10.Axis = Axises[9];
           
        }
    }
}

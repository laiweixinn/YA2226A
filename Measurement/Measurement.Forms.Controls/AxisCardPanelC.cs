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

namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class AxisCardPanelC : PageUC.BasePageUC
    {
        public AxisCardPanelC()
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

        public void Init(MeasurementAxis[] Axises)
        {
            axisMovePanel1.Axis = Axises[0];
            axisMovePanel2.Axis = Axises[1];
            axisMovePanel3.Axis = Axises[2];
            axisMovePanel4.Axis = Axises[3];
            axisMovePanel5.Axis = Axises[4];
            axisMovePanel6.Axis = Axises[5];
            if (Axises.Length > 6)
            {
                axisMovePanel7.Axis = Axises[6];
            }


            if (Axises.Length > 7)
            {
                axisMovePanel8.Axis = Axises[7];
            }

        }


    }
}

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
    public partial class AxisCardPanel :PageUC.BasePageUC
    {
        public AxisCardPanel()
        {
            InitializeComponent();
           // Init();
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
            axisMovePanel11.Axis = Axises[10];
            axisMovePanel12.Axis = Axises[11];
      }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LZ.CNC.Measurement.Core;

namespace LZ.CNC.Measurement.Forms.Controls
{
    public partial class CardModulC :PageUC.BasePageUC
    {
        public CardModulC()
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

        public override void Init()
        {
            MeasurementConfig config = MeasurementContext.Config;

            ioSetPanel48.IO = config.LeftBend_UPOPTControl_IOOutEx;
            ioSetPanel47.IO = config.MidBend_UPOPTControl_IOOutEx;
            ioSetPanel46.IO = config.RightBend_UPOPTControl_IOOutEx;
            ioSetPanel45.IO = config.SMStation_OPTIOOutEx;
            ioSetPanel44.IO = config.Bend_FrontGate1_IOOutEx;
            ioSetPanel43.IO = config.Bend_BackGate1_IOOutEx;
            ioSetPanel42.IO = config.Bend_SideGate1_IOOutEx;
            //ioSetPanel41.IO = config.SMStation_BackGate1_IOOutEx;
            //ioSetPanel40.IO = config.Bend_SideGate1_IOOutEx;
            //ioSetPanel39.IO = config.Bend_BackGate1_IOOutEx;
            ioSetPanel38.IO = config.SendUpstream_Safe_IOOutEx;
            ioSetPanel37.IO = config.SendUpstream_Request_IOOutEx;
            ioSetPanel36.IO = config.SendUpstream_Finish_IOOutEx;
            ioSetPanel35.IO = config.SendUpstream_Spare_IOOutEx;
            //ioSetPanel34.IO = config.DischargZbrak_IOOutEx;
            //ioSetPanel33.IO = config.CardCOutEx16;


            ioSetPanel16.IO = config.BendFrontGate1IOInEx;
            ioSetPanel15.IO = config.BendFrontGate2IOInEx;
            ioSetPanel14.IO = config.BendBackGate1IOInEx;
            ioSetPanel13.IO = config.BendBackGate2IOInEx;
            ioSetPanel12.IO = config.BendSideGate1IOInEx;
            ioSetPanel11.IO = config.BendSideGate2IOInEx;
            //ioSetPanel10.IO = config.BendFrontGate1IOInEx;
            //ioSetPanel9.IO =  config.BendFrontGate2IOInEx;
            //ioSetPanel8.IO =  config.BendBackGate1IOInEx;
            //ioSetPanel7.IO =  config.BendBackGate2IOInEx;
            ioSetPanel6.IO =  config.BendION_AlarmIOInEx;
            ioSetPanel5.IO =  config.InputAir_IOInEx;
            ioSetPanel4.IO =  config.InputVacumn_IOInEx;
            ioSetPanel3.IO =  config.ReceiveUpstream_Safe_IOInEx;
            ioSetPanel2.IO =  config.ReceiveUpstream_Request_IOInEx;
            ioSetPanel1.IO = config.ReceiveUpstream_Finish_IOInEx;
            ioSetPanel_D_16InEx.IO = config.ReceiveUpstream_Spare_IOInEx;
            ioSetPanel_D_17InEx.IO = config.NGLineReachIOInEx;
            ioSetPanel_D_18InEx.IO = config.NGLineHaveIOInEx;
            ioSetPanel_D_19InEx.IO = config.NGLineFullIOInEx;
        }

        public override void Save()
        {
            MeasurementConfig config = MeasurementContext.Config;


            foreach (IOSetPanel item in panel1.Controls)
            {
                item.Save();
            }

            foreach (IOSetPanel item in panel2.Controls)
            {
                item.Save();
            }
            config.Save();
        }

        private void ioset_changed(object sender, EventArgs e)
        {
            if (IsInited)
            {
                OnValuedChanged();
            }
        }




    }
}

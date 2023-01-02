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
    public partial class CardBIOSet :PageUC.BasePageUC
    {
        public CardBIOSet()
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

            ioSetPanel48.IO = config.RightSM_RollerCylinder_IOOut;
            ioSetPanel47.IO = config.RightSM_GlueLockCylinder_IOOut;
            //ioSetPanel46.IO = config.IOOut14;
            ioSetPanel45.IO = config.RedLightIOOut;
            ioSetPanel44.IO = config.YellowLightIOOut;
            ioSetPanel43.IO = config.GreenLightIOOut;
            ioSetPanel42.IO = config.BuzzerIOOut;
            ioSetPanel41.IO = config.StartBlueLightIOOut;
            ioSetPanel40.IO = config.ResetYellowLightIOOut;
            //ioSetPanel39.IO = config.Transfer_Suckvacuum_IOOut;
            //ioSetPanel38.IO = config.Transfer_Blowvacuum_IOOut;
            //ioSetPanel37.IO = config.Feed_RotateSuck_IOOut;
            //ioSetPanel36.IO = config.Feed_RotateBreakVacuum_IOOut;
            //ioSetPanel35.IO = config.Feed_RotateFPCSuck_IOOut;
            //ioSetPanel34.IO = config.IOOut14;
            //ioSetPanel33.IO = config.IOOut15;


            ioSetPanel16.IO = config.MidSM_RollerUD_CylinderDownIOIn;
            ioSetPanel15.IO = config.RightSMVacuumIOIn;
            ioSetPanel14.IO = config.RightSMMPVacuumIOIn;
            ioSetPanel13.IO = config.RightSMGlueOpticalIOIn;
            ioSetPanel12.IO = config.RightSM_LR_CylinerLeftIOIn;
            ioSetPanel11.IO = config.RightSM_LR_CylinderRightIOIn;
            ioSetPanel10.IO = config.RightSM_FB_CylinderBackIOIn;
            ioSetPanel9.IO = config.RightSM_FB_CylinderFrontIOIn;
            ioSetPanel8.IO = config.RightSM_UD_CylinderUPIOIn;
            ioSetPanel7.IO = config.RightSM_UD_CylinderDownIOIn;
            ioSetPanel6.IO = config.RightSM_GlueUDCylinderUPIOIn;
            ioSetPanel5.IO = config.EmgStopCard1IOIn;
            //ioSetPanel4.IO = config.RightSM_RollerUD_CylinderUPIOInEx;
            //ioSetPanel3.IO = config.SM_CCDXAxis_Alarm_IOInEx;
            //ioSetPanel2.IO = config.DischargeAxisZservorAlarmIOIn;
            //ioSetPanel1.IO = config.EmgStopCard1IOIn;
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

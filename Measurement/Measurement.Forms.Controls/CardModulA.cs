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
    public partial class CardModulA :PageUC.BasePageUC
    {

        public CardModulA()
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

            ioSetPanel48.IO = config.MidSM_StgBlowVacuum_IOOutEx;
            ioSetPanel47.IO = config.MidSM_StgReduceVacuum_IOOutEx;
            ioSetPanel46.IO = config.MidSM_LRCylinder_IOOutEx;
            ioSetPanel45.IO = config.MidSM_FBCylinder_IOOutEx;
            ioSetPanel44.IO = config.MidSM_UDCylinder_IOOutEx;
            ioSetPanel43.IO = config.MidSM_GlueCylinder_IOOutEx;
            ioSetPanel42.IO = config.MidSM_RollerCylinder_IOOutEx;
            ioSetPanel41.IO = config.MidSM_GlueLockCylinder_IOOutEx;
            ioSetPanel40.IO = config.RightSM_StgVacuum_IOOutEx;
            ioSetPanel39.IO = config.RightSM_StgFPCVacuum_IOOutEx;
            ioSetPanel38.IO = config.RightSM_StgBlowVacuum_IOOutEx;
            ioSetPanel37.IO = config.RightSM_StgReduceVacuum_IOOutEx;
            ioSetPanel36.IO = config.RightSM_LRCylinder_IOOutEx;
            ioSetPanel35.IO = config.RightSM_FBCylinder_IOOutEx;
            ioSetPanel34.IO = config.RightSM_UDCylinder_IOOutEx;
            ioSetPanel33.IO = config.RightSM_GlueCylinder_IOOutEx;

            ioSetPanel16.IO = config.LeftSM_WAxis_ServoAlarmIOInEx;
            ioSetPanel15.IO = config.MidSM_WAxis_ServoAlarmIOInEx;
            ioSetPanel14.IO = config.RightSM_WAxis_ServoAlarmIOInEx;
            ioSetPanel13.IO = config.LeftSM_UD_CylinderDownIOInEx;
            ioSetPanel12.IO = config.LeftSM_RollerUD_CylinderDownIOInEx;
            ioSetPanel11.IO = config.MidSMVacuumIOInEx;
            ioSetPanel10.IO = config.MidSMMPVacuumIOInEx;
            ioSetPanel9.IO = config.MidSMGlueOpticalIOInEx;
            ioSetPanel8.IO = config.MidSM_LR_CylinderRightIOInEx;
            ioSetPanel7.IO = config.MidSM_LR_CylinerLeftIOInEx;
            ioSetPanel6.IO = config.MidSM_FB_CylinderBackIOInEx;
            ioSetPanel5.IO = config.MidSM_FB_CylinderFrontIOInEx;
            ioSetPanel4.IO = config.MidSM_UD_CylinderUPIOInEx;
            ioSetPanel3.IO = config.MidSM_UD_CylinderDownIOInEx;
            ioSetPanel2.IO = config.MidSM_GlueUD_CylinderUPIOInEx;
            ioSetPanel1.IO = config.MidSM_GlueUD_CylinderDownIOInEx;
            ioSetPanel_A_16InEx.IO = config.MidSM_RollerUD_CylinderUPIOInEx;
            ioSetPanel_A_17InEx.IO = config.LeftSM_GlueUD_CylinderUPIOInEx;
            ioSetPanel_A_18InEx.IO = config.LeftSM_GlueUD_CylinderDownIOInEx;
            ioSetPanel_A_19InEx.IO = config.LeftSM_RollerUD_CylinderUpIOInEx;

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

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }
    }
}

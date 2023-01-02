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
    public partial class CardAIOSet :PageUC.BasePageUC
    {
        public CardAIOSet()
        {
            InitializeComponent();
            //Init();
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

            //ioSetPanel48.IO = config.SuplyBeltIOOut;
            ioSetPanel47.IO = config.LoadVacuumIOOut;
            ioSetPanel46.IO = config.LoadFPCVacuumIOOut;
            ioSetPanel45.IO = config.LoadBlowVacuumIOOut;
            ioSetPanel44.IO = config.LeftSM_StgVacuum_IOOut;
            ioSetPanel43.IO = config.LeftSM_StgFPCVacuum_IOOut;
            ioSetPanel42.IO = config.LeftSM_StgBlowVacuum_IOOut;
            ioSetPanel41.IO = config.LeftSM_StgReduceVacuum_IOOut;
            ioSetPanel40.IO = config.LeftSM_LRCylinder_IOOut;
            ioSetPanel39.IO = config.LeftSM_FBCylinder_IOOut;
            ioSetPanel38.IO = config.LeftSM_UDCylinder_IOOut;
            ioSetPanel37.IO = config.LeftSM_GlueCylinder_IOOut;
            ioSetPanel36.IO = config.LeftSM_RollerCylinder_IOOut;
            ioSetPanel35.IO = config.LeftSM_GlueLockCylinder_IOOut;
            ioSetPanel34.IO = config.MidSM_StgVacuum_IOOut;
            ioSetPanel33.IO = config.MidSM_StgFPCVacuum_IOOut;
            ioSetPanel32.IO = config.Feed_UDCylinderUP_IOOut;
            ioSetPanel31.IO = config.Feed_UDCylinderDown_IOOut;
            ioSetPanel30.IO = config.TearAOIBlowCylinder;
            ioSetPanel29.IO = config.CIMCOR_Cylinder;

            //ioSetPanel16.IO = config.BeltOpticalIOIN;
            ioSetPanel15.IO = config.LoadVacuumIOIn;
            ioSetPanel14.IO = config.LoadfFPCVacuumIOIn;
            ioSetPanel13.IO = config.LeFTSMVacuumIOIn ;
            ioSetPanel12.IO = config.LeftSMMPVacuumIOIn;
            ioSetPanel11.IO = config.LeftSMGlueOpticalIOIn;
            ioSetPanel10.IO = config.LeftSM_LR_CylinderRightIOIn;
            ioSetPanel9.IO = config.LeftSM_LR_CylinderLeftIOIn;
            ioSetPanel8.IO = config.LeftSM_FB_CylinderBackIOIn;
            ioSetPanel7.IO = config.LeftSM_FB_CylinderFrontIOIn;
            ioSetPanel6.IO = config.LeftSM_UD_CylinderUPIOIn;
            ioSetPanel5.IO = config.EmgStopCard0IoIn;
            //ioSetPanel4.IO = config.EmgStopCard0IoIn;       
            //ioSetPanel4.IO = config.LeftSM_GlueUD_CylinderUPIOIn;
            //ioSetPanel3.IO = config.LeftSM_GlueUD_CylinderDownIOIn;
            //ioSetPanel2.IO = config.LeftSM_RollerUD_CylinderUPIOIn;
           // ioSetPanel1.IO = config.EmgStopCard0IoIn;
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

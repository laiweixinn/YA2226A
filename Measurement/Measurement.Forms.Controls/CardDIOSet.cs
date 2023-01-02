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
    public partial class CardDIOSet :PageUC.BasePageUC
    {
        public CardDIOSet()
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

            ioSetPanel48.IO = config.RightBend_PressCylinderUp_IOOut;
            ioSetPanel47.IO = config.RightBend_PressCylinderDown_IOOut;
            ioSetPanel46.IO = config.Transfer_Suckvacuum_IOOut;
            ioSetPanel45.IO = config.Transfer_FPCSuckvacuum_IOOut;
            ioSetPanel44.IO = config.Transfer_Blowvacuum_IOOut;
            ioSetPanel43.IO = config.Discharge_Suckvacuum_IOOut;
            ioSetPanel42.IO = config.Discharge_FPCSuckvacuum_IOOut;
            ioSetPanel41.IO = config.Discharge_Blowvacuum_IOOut;
            ioSetPanel40.IO = config.NGlineBeltIOOut;
            ioSetPanel39.IO = config.DischargeLineBeltIOOut;
            //ioSetPanel38.IO = config.TearAOIBlowCylinder;
            //ioSetPanel37.IO = config.NGLinePushCylinder;
            //ioSetPanel36.IO = config.Feed_UDCylinderUP_IOOut;
            //ioSetPanel35.IO = config.Feed_UDCylinderDown_IOOut;
            //ioSetPanel34.IO = config.Feed_RotateUDCylinderUp_IOOut;
            //ioSetPanel33.IO = config.Feed_RotateUDCylinderDown_IOOut;


            ioSetPanel16.IO = config.MidBend_PressCylinder_DownIOIn;
            //ioSetPanel15.IO = config.RightBend_FPCOptical_IOIn;
            ioSetPanel14.IO = config.RightBend_stgVacuum_IOIn;
            ioSetPanel13.IO = config.DischargeLine_OpticalSensorHave_IOIn;
            ioSetPanel12.IO = config.DischargeLine_OpticalSensorFull_IOIn;
            ioSetPanel11.IO = config.Discharge_FPCVacuumIOIn;
            ioSetPanel10.IO = config.RightBend_PressCylinder_UPIOIn;
            ioSetPanel9.IO = config.RightBend_PressCylinder_DownIOIn;
            ioSetPanel8.IO = config.TransferVacuumIOIn;
            ioSetPanel7.IO = config.TransferFPCVacuumIOIn;
            ioSetPanel6.IO = config.DischargeVacuumIOIn;
            ioSetPanel5.IO = config.EmgStopCard3IOIn;
            //ioSetPanel4.IO = config.DischargeLine_OpticalSensorHave_IOIn;
            //ioSetPanel3.IO = config.DischargeLine_OpticalSensorFull_IOIn;
            //ioSetPanel2.IO = config.Feed_RotateUpDownCylider_DownIOIn;
            //ioSetPanel1.IO = config.Feed_RotateUpDownCylinder_UpIOIn;
            //ioSetPanel18.IO = config.Feed_RotateVacuumCheckIOIn;
            //ioSetPanel17.IO = config.Discharge_FPCVacuumIOIn;



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

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
    public partial class CardCIOSet :PageUC.BasePageUC
    {
        public CardCIOSet()
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

            ioSetPanel48.IO = config.LeftBend_SuckVacuum_IOOut;
            ioSetPanel47.IO = config.LeftBend_BlowVacuum_IOOut;
            ioSetPanel46.IO = config.LeftBend_ClawCylinderOut_IOOut;
            //ioSetPanel45.IO = config.UpstreamSuplyBeltIOOut;
            ioSetPanel44.IO = config.LeftBend_PressCylinderUp_IOOut;
            ioSetPanel43.IO = config.LeftBend_PressCylinderDown_IOOut;
            ioSetPanel42.IO = config.MidBend_SuckVacuum_IOOut;
            ioSetPanel41.IO = config.MidBend_BlowVacuum_IOOut;
            ioSetPanel40.IO = config.MidBend_ClawCylinderOut_IOOut;
            //ioSetPanel39.IO = config.TransferCylinderUp_IOOut;
            //ioSetPanel38.IO = config.TransferCylinderDown_IOOut;
            ioSetPanel39.IO = config.MidBend_PressCylinderUp_IOOut;
            ioSetPanel38.IO = config.MidBend_PressCylinderDown_IOOut;
            //ioSetPanel37.IO = config.MidBend_PressCylinderUp_IOOut;
            ioSetPanel36.IO = config.RightBend_SuckVacuum_IOOut;
            ioSetPanel35.IO = config.RightBend_BlowVacuum_IOOut;
            ioSetPanel34.IO = config.RightBend_ClawCylinderOut_IOOut;
            ioSetPanel33.IO = config.LeftBend_ClawCylinderBack_IOOut;
            ioSetPanel32.IO = config.MidBend_ClawCylinderBack_IOOut;
            ioSetPanel31.IO = config.RightBend_ClawCylinderBack_IOOut;
            //ioSetPanel30.IO = config.RightBend_ClawCylinderBack_IOOut;


            //ioSetPanel16.IO = config.LeftBend_FPCOptical_IOIn;
            ioSetPanel15.IO = config.LeftBend_stgVacuum_IOIn;
            //ioSetPanel14.IO = config.Feed_UpDownCylinder_UpIOIn;
            //ioSetPanel13.IO = config.Feed_UpDownCylinder_DownIOIn;
            //ioSetPanel12.IO = config.Feed_RotateCylinder_UpIOIn;
            //ioSetPanel11.IO = config.Feed_RotateCylinder_DownIOIn;
            ioSetPanel10.IO = config.LeftBend_PressCylinder_UPIOIn;
            ioSetPanel9.IO = config.LeftBend_PressCylinder_DownIOIn;
            //ioSetPanel8.IO = config.MidBend_FPCOptical_IOIn;
            ioSetPanel7.IO = config.MidBend_stgVacuum_IOIn;
            ioSetPanel6.IO = config.MidBend_PressCylinder_UPIOIn;
            ioSetPanel5.IO = config.EmgStopCard2IOIn;
            //ioSetPanel4.IO = config.UpstreamBeltOpticalIOINEx;
            //ioSetPanel3.IO = config.BeltStartOpticalIOINEx;
            //ioSetPanel2.IO = config.MidBend_PressCylinder_UPIOIn;
            //ioSetPanel1.IO = config.EmgStopCard2IOIn;
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

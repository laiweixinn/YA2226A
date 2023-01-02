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
    public partial class CardModulB : PageUC.BasePageUC
    {
        public CardModulB()
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

            ioSetPanel48.IO = config.Left_WServoOn_IOOutEx;
            ioSetPanel47.IO = config.Mid_WServoOn_IOOutEx;
            ioSetPanel46.IO = config.Right_WServoOn_IOOutEx;
            ioSetPanel45.IO = config.DischargZServoOn_IOOutEx;
            ioSetPanel44.IO = config.SM_AOIServoOn_IOOutEx;
            ioSetPanel43.IO = config.Left_ClearWAlarm_IOOutEx;
            ioSetPanel42.IO = config.Mid_ClearWAlarm_IOOutEx;
            ioSetPanel41.IO = config.Right_ClearWAlarm_IOOutEx;
            ioSetPanel40.IO = config.SMAOIClearAlarm_IOOutEx;
            ioSetPanel39.IO = config.AllStepMotorServoOn_IOOutEx;
            ioSetPanel38.IO = config.TransferCylinderUp_IOOutEx;
            ioSetPanel37.IO = config.TransferCylinderDown_IOOutEx;
            ioSetPanel36.IO = config.SMStation_FrontGate1_IOOutEx;
            ioSetPanel35.IO = config.SMStation_BackGate1_IOOutEx;
            ioSetPanel34.IO = config.DischargZClearAlarm_IOOutEx;
            //ioSetPanel33.IO = config.OutEx13;


            ioSetPanel16.IO = config.RightSM_GlueUD_CylinderDownIOInEx;
            ioSetPanel15.IO = config.RightSM_RollerUD_CylinderUPIOInEx;
            ioSetPanel14.IO = config.SM_CCDXAxis_Alarm_IOInEx;
            ioSetPanel13.IO = config.StepMotorPower_IOInEx;
            ioSetPanel12.IO = config.StartBtnIOInEx;
            ioSetPanel11.IO = config.ResetbtnIOInEx;
            ioSetPanel10.IO = config.Feed_UpDownCylinder_UpIOInEx;
            ioSetPanel9.IO = config.Feed_UpDownCylinder_DownIOInEx;
            ioSetPanel8.IO = config.Transfer_UDCylinderUP_IOInEx;
            ioSetPanel7.IO = config.Transfer_UDCylinderDown_IOInEx;
            ioSetPanel6.IO = config.SMFrontGate1IOInEx;
            ioSetPanel5.IO = config.SMFrontGate2IOInEx;
            ioSetPanel4.IO = config.SMBackGate1IOInEx;
            ioSetPanel3.IO = config.SMBackGate2IOInEx;
            //ioSetPanel2.IO = config.SMFrontGate1IOInEx;
            //ioSetPanel1.IO = config.SMSideGate4IOInEx;
            //ioSetPanel_C_16InEx.IO = config.SMFrontGate1IOInEx;
            ioSetPanel_C_17InEx.IO = config.DischargeAxisZservorAlarmIOInEx;
            ioSetPanel_C_18InEx.IO = config.SMION_AlarmIOInEx;
            ioSetPanel_C_19InEx.IO = config.RightSM_RollerUD_CylinderDownIOInEx;

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

        private void CardModulB_Load(object sender, EventArgs e)
        {

        }
    }
}

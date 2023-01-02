using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LZ.CNC.Measurement.Core;


namespace LZ.CNC.Measurement.Forms
{
    public partial class FrmIO : TabForm
    {
        public FrmIO()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            MeasurementConfig config = MeasurementContext.Config;
            if (config != null)
            {
                //CardA out
                ioStatePanel1.IO = config.IOOut100; //备用IO输出
                ioStatePanel2.IO = config.LoadVacuumIOOut;
                ioStatePanel3.IO = config.LoadFPCVacuumIOOut;
                ioStatePanel4.IO = config.LoadBlowVacuumIOOut;
                ioStatePanel5.IO = config.LeftSM_StgVacuum_IOOut;
                ioStatePanel6.IO = config.LeftSM_StgFPCVacuum_IOOut;
                ioStatePanel7.IO = config.LeftSM_StgBlowVacuum_IOOut;
                ioStatePanel8.IO = config.LeftSM_StgReduceVacuum_IOOut;
                ioStatePanel9.IO = config.LeftSM_LRCylinder_IOOut;
                ioStatePanel10.IO = config.LeftSM_FBCylinder_IOOut;
                ioStatePanel11.IO = config.LeftSM_UDCylinder_IOOut;
                ioStatePanel12.IO = config.LeftSM_GlueCylinder_IOOut;
                ioStatePanel13.IO = config.LeftSM_RollerCylinder_IOOut;
                ioStatePanel14.IO = config.LeftSM_GlueLockCylinder_IOOut;
                ioStatePanel15.IO = config.MidSM_StgVacuum_IOOut;
                ioStatePanel16.IO = config.MidSM_StgFPCVacuum_IOOut;
                ioStatePanel_AOut1.IO= config.Feed_UDCylinderUP_IOOut;
                ioStatePanel_AOut2.IO = config.Feed_UDCylinderDown_IOOut;
                ioStatePanel_AOut3.IO = config.TearAOIBlowCylinder;
                ioStatePanel_AOut4.IO = config.CIMCOR_Cylinder;

                //CardB out
                //ioStatePanel_BOut4.IO = config.IOOut15;
                //ioStatePanel_BOut3.IO = config.IOOut15;
                //ioStatePanel_BOut2.IO = config.IOOut14;
                //ioStatePanel_BOut1.IO = config.IOOut14;

                ioStatePanel17.IO = config.IOOut100; //备用IO输出
                ioStatePanel18.IO = config.IOOut100; //备用IO输出
                ioStatePanel19.IO = config.IOOut100; //备用IO输出
                ioStatePanel20.IO = config.IOOut100; //备用IO输出
                ioStatePanel21.IO = config.IOOut100; //备用IO输出
                ioStatePanel22.IO = config.IOOut100; //备用IO输出
                ioStatePanel23.IO = config.IOOut100; //备用IO输出
                ioStatePanel24.IO = config.ResetYellowLightIOOut;
                ioStatePanel25.IO = config.StartBlueLightIOOut;
                ioStatePanel26.IO = config.BuzzerIOOut;
                ioStatePanel27.IO = config.GreenLightIOOut;
                ioStatePanel28.IO = config.YellowLightIOOut;
                ioStatePanel29.IO = config.RedLightIOOut;
                ioStatePanel30.IO = config.IOOut100; //备用IO输出
                ioStatePanel31.IO = config.RightSM_GlueLockCylinder_IOOut;
                ioStatePanel32.IO = config.RightSM_RollerCylinder_IOOut;


                //CardC out
                //ioStatePanel_COut4.IO = config.MidBend_ClawCylinderBack_IOOut;
                //ioStatePanel_COut3.IO = config.MidBend_ClawCylinderBack_IOOut;
                ioStatePanel_COut2.IO = config.RightBend_ClawCylinderBack_IOOut;
                ioStatePanel_COut1.IO = config.MidBend_ClawCylinderBack_IOOut;
                ioStatePanel33.IO = config.LeftBend_ClawCylinderBack_IOOut;
                ioStatePanel34.IO = config.RightBend_ClawCylinderOut_IOOut;
                ioStatePanel35.IO = config.RightBend_BlowVacuum_IOOut;
                ioStatePanel36.IO = config.RightBend_SuckVacuum_IOOut;
                ioStatePanel37.IO = config.IOOut100; //备用IO输出
                ioStatePanel38.IO = config.MidBend_PressCylinderDown_IOOut;
                ioStatePanel39.IO = config.MidBend_PressCylinderUp_IOOut;
                ioStatePanel40.IO = config.MidBend_ClawCylinderOut_IOOut;
                ioStatePanel41.IO = config.MidBend_BlowVacuum_IOOut;
                ioStatePanel42.IO = config.MidBend_SuckVacuum_IOOut;
                ioStatePanel43.IO = config.LeftBend_PressCylinderDown_IOOut;
                ioStatePanel44.IO = config.LeftBend_PressCylinderUp_IOOut;
                ioStatePanel45.IO = config.IOOut100; //备用IO输出
                ioStatePanel46.IO = config.LeftBend_ClawCylinderOut_IOOut;
                ioStatePanel47.IO = config.LeftBend_BlowVacuum_IOOut;
                ioStatePanel48.IO = config.LeftBend_SuckVacuum_IOOut;


                //CardD Out
                //ioStatePanel_DOut4.IO = config.IOOut14;
                //ioStatePanel_DOut3.IO = config.IOOut14;
                //ioStatePanel_DOut2.IO = config.IOOut14;
                //ioStatePanel_DOut1.IO = config.IOOut14;

                ioStatePanel49.IO = config.IOOut100; //备用IO输出
                ioStatePanel50.IO = config.IOOut100; //备用IO输出
                ioStatePanel51.IO = config.IOOut100; //备用IO输出
                ioStatePanel52.IO = config.IOOut100; //备用IO输出
                ioStatePanel53.IO = config.IOOut100; //备用IO输出
                ioStatePanel54.IO = config.IOOut100; //备用IO输出
                ioStatePanel55.IO = config.DischargeLineBeltIOOut;
                ioStatePanel56.IO = config.NGlineBeltIOOut;
                ioStatePanel57.IO = config.Discharge_Blowvacuum_IOOut;
                ioStatePanel58.IO = config.Discharge_FPCSuckvacuum_IOOut;
                ioStatePanel59.IO = config.Discharge_Suckvacuum_IOOut;
                ioStatePanel60.IO = config.Transfer_Blowvacuum_IOOut;
                ioStatePanel61.IO = config.Transfer_FPCSuckvacuum_IOOut;
                ioStatePanel62.IO = config.Transfer_Suckvacuum_IOOut;
                ioStatePanel63.IO = config.RightBend_PressCylinderDown_IOOut;
                ioStatePanel64.IO = config.RightBend_PressCylinderUp_IOOut;


                //CardA In

                //ioStatePanel66.IO = config.LeftSM_WAxis_Alarm_IOIn;
                //ioStatePanel67.IO = config.MidSM_WAxis_Alarm_IOIn ;
                //ioStatePanel68.IO = config.RightSM_WAxis_Alarm_IOIn;
                //ioStatePanel69.IO = config.LeftSM_UD_CylinderDownIOIn;



                ioStatePanel69.IO = config.EmgStopCard0IoIn;
                ioStatePanel70.IO = config.LeftSM_UD_CylinderUPIOIn;
                ioStatePanel71.IO = config.LeftSM_FB_CylinderFrontIOIn;
                ioStatePanel72.IO = config.LeftSM_FB_CylinderBackIOIn;
                ioStatePanel73.IO = config.LeftSM_LR_CylinderRightIOIn;
                ioStatePanel74.IO = config.LeftSM_LR_CylinderLeftIOIn;
                ioStatePanel75.IO = config.LeftSMGlueOpticalIOIn;
                ioStatePanel76.IO = config.LeftSMMPVacuumIOIn;
                ioStatePanel77.IO = config.LeFTSMVacuumIOIn;
                ioStatePanel78.IO = config.LoadfFPCVacuumIOIn;
                ioStatePanel79.IO = config.LoadVacuumIOIn;
                ioStatePanel80.IO = config.IOIn100;//备用IO输入


                //CardB  In
                //ioStatePanel81.IO = config.EmgStopCard1IOIn;
                //ioStatePanel82.IO = config.DischargeAxisZservorAlarmIOIn;
                //ioStatePanel83.IO = config.SM_CCDXAxis_Alarm_IOIn;                    
                //ioStatePanel84.IO = config.RightSM_RollerUD_CylinderUPIOIn;
                ioStatePanel85.IO = config.EmgStopCard1IOIn;
                ioStatePanel86.IO = config.RightSM_GlueUDCylinderUPIOIn;               
                ioStatePanel87.IO = config.RightSM_UD_CylinderDownIOIn;
                ioStatePanel88.IO = config.RightSM_UD_CylinderUPIOIn;
                ioStatePanel89.IO = config.RightSM_FB_CylinderFrontIOIn;
                ioStatePanel90.IO = config.RightSM_FB_CylinderBackIOIn;
                ioStatePanel91.IO = config.RightSM_LR_CylinderRightIOIn;
                ioStatePanel92.IO = config.RightSM_LR_CylinerLeftIOIn;
                ioStatePanel93.IO = config.RightSMGlueOpticalIOIn;
                ioStatePanel94.IO = config.RightSMMPVacuumIOIn;
                ioStatePanel95.IO = config.RightSMVacuumIOIn;
                ioStatePanel96.IO = config.MidSM_RollerUD_CylinderDownIOIn;


                //CardC In
                //ioStatePanel97.IO = config.EmgStopCard2IOIn;
                //ioStatePanel98.IO = config.MidBend_PressCylinder_UPIOIn;
                //ioStatePanel99.IO = config.BeltStartOpticalIOIN;
                //ioStatePanel100.IO = config.UpstreamBeltOpticalIOIN;
                //ioStatePanel101.IO = config.Feed_RotateFPCVacuumCheckIOIn;
                //ioStatePanel102.IO = config.Feed_RotateVacuumCheckIOIn;

                ioStatePanel101.IO = config.EmgStopCard2IOIn;
                ioStatePanel102.IO = config.MidBend_PressCylinder_UPIOIn;
                ioStatePanel103.IO = config.MidBend_stgVacuum_IOIn;
                ioStatePanel104.IO = config.IOIn100;//备用IO输入
                ioStatePanel105.IO = config.LeftBend_PressCylinder_DownIOIn;
                ioStatePanel106.IO = config.LeftBend_PressCylinder_UPIOIn;
                ioStatePanel107.IO = config.IOIn100;//备用IO输入
                ioStatePanel108.IO = config.IOIn100;//备用IO输入
                ioStatePanel109.IO = config.IOIn100;//备用IO输入
                ioStatePanel110.IO = config.IOIn100;//备用IO输入
                ioStatePanel111.IO = config.LeftBend_stgVacuum_IOIn;
                ioStatePanel112.IO = config.IOIn100;//备用IO输入


                //CardD In

                ioStatePanel_D_17.IO = config.IOIn100;//备用IO输入
                ioStatePanel_D_16.IO = config.IOIn100;//备用IO输入
                ioStatePanel113.IO = config.IOIn100;//备用IO输入
                ioStatePanel114.IO = config.IOIn100;//备用IO输入
                ioStatePanel115.IO = config.IOIn100;//备用IO输入
                ioStatePanel116.IO = config.IOIn100;//备用IO输入
                ioStatePanel117.IO = config.EmgStopCard3IOIn;
                ioStatePanel118.IO = config.DischargeVacuumIOIn;
                ioStatePanel119.IO = config.TransferFPCVacuumIOIn;
                ioStatePanel120.IO = config.TransferVacuumIOIn;
                ioStatePanel121.IO = config.RightBend_PressCylinder_DownIOIn;
                ioStatePanel122.IO = config.RightBend_PressCylinder_UPIOIn;
                ioStatePanel123.IO = config.Discharge_FPCVacuumIOIn;
                ioStatePanel124.IO = config.DischargeLine_OpticalSensorFull_IOIn;
                ioStatePanel125.IO = config.DischargeLine_OpticalSensorHave_IOIn;
                ioStatePanel126.IO = config.RightBend_stgVacuum_IOIn;
                ioStatePanel127.IO = config.IOIn100;//备用IO输入
                ioStatePanel128.IO = config.MidBend_PressCylinder_DownIOIn;


                //CardModulA  Out
                ioStatePanel145.IO = config.RightSM_GlueCylinder_IOOutEx;
                ioStatePanel146.IO = config.RightSM_UDCylinder_IOOutEx;
                ioStatePanel147.IO = config.RightSM_FBCylinder_IOOutEx;
                ioStatePanel148.IO = config.RightSM_LRCylinder_IOOutEx;
                ioStatePanel149.IO = config.RightSM_StgReduceVacuum_IOOutEx;
                ioStatePanel150.IO = config.RightSM_StgBlowVacuum_IOOutEx;
                ioStatePanel151.IO = config.RightSM_StgFPCVacuum_IOOutEx;
                ioStatePanel152.IO = config.RightSM_StgVacuum_IOOutEx;
                ioStatePanel153.IO = config.MidSM_GlueLockCylinder_IOOutEx;
                ioStatePanel154.IO = config.MidSM_RollerCylinder_IOOutEx;
                ioStatePanel155.IO = config.MidSM_GlueCylinder_IOOutEx;
                ioStatePanel156.IO = config.MidSM_UDCylinder_IOOutEx;
                ioStatePanel157.IO = config.MidSM_FBCylinder_IOOutEx;
                ioStatePanel158.IO = config.MidSM_LRCylinder_IOOutEx;
                ioStatePanel159.IO = config.MidSM_StgReduceVacuum_IOOutEx;
                ioStatePanel160.IO = config.MidSM_StgBlowVacuum_IOOutEx;


                //CardModulB Out
                ioStatePanel161.IO = config.OutEx13;
                ioStatePanel162.IO = config.SMStation_BackGate1_IOOutEx;
                ioStatePanel163.IO = config.SMStation_FrontGate1_IOOutEx;
                ioStatePanel164.IO = config.TransferCylinderDown_IOOutEx;
                ioStatePanel165.IO = config.TransferCylinderUp_IOOutEx;
                ioStatePanel166.IO = config.AllStepMotorServoOn_IOOutEx;
                ioStatePanel167.IO = config.SMAOIClearAlarm_IOOutEx;
                ioStatePanel168.IO = config.Right_ClearWAlarm_IOOutEx;
                ioStatePanel169.IO = config.Mid_ClearWAlarm_IOOutEx;
                ioStatePanel170.IO = config.Left_ClearWAlarm_IOOutEx;
                ioStatePanel171.IO = config.SM_AOIServoOn_IOOutEx;        
                ioStatePanel173.IO = config.DischargZServoOn_IOOutEx;
                ioStatePanel174.IO = config.Right_WServoOn_IOOutEx;
                ioStatePanel175.IO = config.Mid_WServoOn_IOOutEx;
                ioStatePanel176.IO = config.Left_WServoOn_IOOutEx;


                //CardModulC Out
                ioStatePanel177.IO = config.SendUpstream_Spare_IOOutEx;            
                ioStatePanel180.IO = config.SendUpstream_Finish_IOOutEx;
                ioStatePanel181.IO = config.SendUpstream_Request_IOOutEx;
                ioStatePanel182.IO = config.SendUpstream_Safe_IOOutEx;
                ioStatePanel183.IO = config.OutEx13;
                ioStatePanel184.IO = config.OutEx13;
                ioStatePanel185.IO = config.OutEx13;
                ioStatePanel186.IO = config.Bend_SideGate1_IOOutEx;
                ioStatePanel187.IO = config.Bend_BackGate1_IOOutEx;
                ioStatePanel188.IO = config.Bend_FrontGate1_IOOutEx;
                ioStatePanel189.IO = config.SMStation_OPTIOOutEx;
                ioStatePanel190.IO = config.RightBend_UPOPTControl_IOOutEx;
                ioStatePanel191.IO = config.MidBend_UPOPTControl_IOOutEx;
                ioStatePanel192.IO = config.LeftBend_UPOPTControl_IOOutEx;


                //CardModulA In         
                ioStatePanel_A19_ExIn.IO = config.LeftSM_RollerUD_CylinderUpIOInEx;
                ioStatePanel_A18_ExIn.IO = config.LeftSM_GlueUD_CylinderDownIOInEx;
                ioStatePanel_A17_ExIn.IO = config.LeftSM_GlueUD_CylinderUPIOInEx;
                ioStatePanel_A16_ExIn.IO = config.MidSM_RollerUD_CylinderUPIOInEx; ;
                ioStatePanel129.IO = config.MidSM_GlueUD_CylinderDownIOInEx;
                ioStatePanel130.IO = config.MidSM_GlueUD_CylinderUPIOInEx;
                ioStatePanel131.IO = config.MidSM_UD_CylinderDownIOInEx;
                ioStatePanel132.IO = config.MidSM_UD_CylinderUPIOInEx;
                ioStatePanel133.IO = config.MidSM_FB_CylinderFrontIOInEx;
                ioStatePanel134.IO = config.MidSM_FB_CylinderBackIOInEx;
                ioStatePanel135.IO = config.MidSM_LR_CylinerLeftIOInEx;
                ioStatePanel136.IO = config.MidSM_LR_CylinderRightIOInEx;
                ioStatePanel137.IO = config.MidSMGlueOpticalIOInEx;
                ioStatePanel138.IO = config.MidSMMPVacuumIOInEx;
                ioStatePanel139.IO = config.MidSMVacuumIOInEx;
                ioStatePanel140.IO = config.LeftSM_RollerUD_CylinderDownIOInEx;
                ioStatePanel141.IO = config.LeftSM_UD_CylinderDownIOInEx;
                ioStatePanel142.IO = config.RightSM_WAxis_ServoAlarmIOInEx;
                ioStatePanel143.IO = config.MidSM_WAxis_ServoAlarmIOInEx;
                ioStatePanel144.IO = config.LeftSM_WAxis_ServoAlarmIOInEx;


                //CardModulB In

                ioStatePanel_B19_Ex.IO = config.RightSM_RollerUD_CylinderDownIOInEx;
                ioStatePanel_B18_Ex.IO = config.SMION_AlarmIOInEx;
                ioStatePanel_B17_Ex.IO = config.DischargeAxisZservorAlarmIOInEx;
                ioStatePanel_B16_Ex.IO = config.CardInEx100;//备用Ex输入
                ioStatePanel193.IO = config.CardInEx100;//备用Ex输入
                ioStatePanel194.IO = config.CardInEx100;//备用Ex输入
                ioStatePanel195.IO = config.SMBackGate2IOInEx;
                ioStatePanel196.IO = config.SMBackGate1IOInEx;
                ioStatePanel197.IO = config.SMFrontGate2IOInEx;
                ioStatePanel198.IO = config.SMFrontGate1IOInEx;
                ioStatePanel199.IO = config.Transfer_UDCylinderDown_IOInEx;
                ioStatePanel200.IO = config.Transfer_UDCylinderUP_IOInEx;
                ioStatePanel201.IO = config.Feed_UpDownCylinder_DownIOInEx;
                ioStatePanel202.IO = config.Feed_UpDownCylinder_UpIOInEx;
                ioStatePanel203.IO = config.ResetbtnIOInEx;
                ioStatePanel204.IO = config.StartBtnIOInEx;
                ioStatePanel205.IO = config.StepMotorPower_IOInEx;
                ioStatePanel206.IO = config.SM_CCDXAxis_Alarm_IOInEx;
                ioStatePanel207.IO = config.RightSM_RollerUD_CylinderUPIOInEx;
                ioStatePanel208.IO = config.RightSM_GlueUD_CylinderDownIOInEx;



                //CardModulC In
                ioStatePanel_C19_Ex.IO = config.NGLineFullIOInEx;
                ioStatePanel_C18_Ex.IO = config.NGLineHaveIOInEx;
                ioStatePanel_C17_Ex.IO = config.NGLineReachIOInEx;
                ioStatePanel_C16_Ex.IO = config.ReceiveUpstream_Spare_IOInEx;
                ioStatePanel209.IO = config.ReceiveUpstream_Finish_IOInEx;
                ioStatePanel210.IO = config.ReceiveUpstream_Request_IOInEx;
                ioStatePanel211.IO = config.ReceiveUpstream_Safe_IOInEx;
                ioStatePanel212.IO = config.InputVacumn_IOInEx;
                ioStatePanel213.IO = config.InputAir_IOInEx;
                ioStatePanel214.IO = config.BendION_AlarmIOInEx;
                ioStatePanel215.IO = config.CardInEx100;//备用Ex输入
                ioStatePanel216.IO = config.CardInEx100;//备用Ex输入
                ioStatePanel217.IO = config.CardInEx100;//备用Ex输入
                ioStatePanel218.IO = config.CardInEx100;//备用Ex输入
                ioStatePanel219.IO = config.BendSideGate2IOInEx;
                ioStatePanel220.IO = config.BendSideGate1IOInEx;
                ioStatePanel221.IO = config.BendBackGate2IOInEx;
                ioStatePanel222.IO = config.BendBackGate1IOInEx;
                ioStatePanel223.IO = config.BendFrontGate2IOInEx;
                ioStatePanel224.IO = config.BendFrontGate1IOInEx;

                //与上游设备交互 In 
                InteractiveSignal_In0.IO = config.ReceiveUpstream_Safe_IOInEx;
                InteractiveSignal_In1.IO = config.ReceiveUpstream_Request_IOInEx;
                InteractiveSignal_In2.IO = config.ReceiveUpstream_Finish_IOInEx;
                InteractiveSignal_In3.IO = config.ReceiveUpstream_Spare_IOInEx;

                //与上游设备交互 Out
                InteractiveSignal_Out0.IO = config.SendUpstream_Safe_IOOutEx;
                InteractiveSignal_Out1.IO = config.SendUpstream_Request_IOOutEx;
                InteractiveSignal_Out2.IO = config.SendUpstream_Finish_IOOutEx;
                InteractiveSignal_Out3.IO = config.SendUpstream_Spare_IOOutEx;
            }

        }

        protected override void OnLoginChanged()
        {
            cTabControl1.Enabled = (MeasurementContext.UesrManage.LoginType != LZ.CNC.UserLevel.LoginTypes.None);
        }
    }
}

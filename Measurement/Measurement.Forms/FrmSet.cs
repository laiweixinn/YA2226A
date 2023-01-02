using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LZ.CNC.Measurement;
using DY.Core;
using DY.Core.Forms;
using LZ.CNC.Measurement.Core.Motions;
using LZ.CNC.Measurement.Core;

namespace LZ.CNC.Measurement.Forms
{
    public partial class FrmSet : TabForm
    {

        MeasurementData.RecipeDataItem recipe = null;
        MeasurementAxis[] Axises_CardB;

        public FrmSet()
        {
            InitializeComponent();

            List<MeasurementAxis> lisAxiseTemp = new List<MeasurementAxis>();

            lisAxiseTemp.Add(MeasurementContext.Worker.Axis_RightSM_Z);
            lisAxiseTemp.Add(MeasurementContext.Worker.Axis_Load_X);

            if (!MeasurementContext.Config.IsLoadZCylinder)
            {
                lisAxiseTemp.Add(MeasurementContext.Worker.Axis_Load_Z);
            }

            lisAxiseTemp.Add(MeasurementContext.Worker.Axis_Transfer_X);

            if (!MeasurementContext.Config.IsTransferZCylinder)
            {
                lisAxiseTemp.Add(MeasurementContext.Worker.Axis_Transfer_Z);
            }

            lisAxiseTemp.Add(MeasurementContext.Worker.Axis_LeftBend_CCDX);
            lisAxiseTemp.Add(MeasurementContext.Worker.Axis_LeftBend_DWX);
            lisAxiseTemp.Add(MeasurementContext.Worker.Axis_LeftBend_DWY);
            Axises_CardB = lisAxiseTemp.ToArray();

            Init();
            InitBendPara();
        }



        MeasurementAxis[] Axises_CardA = new MeasurementAxis[]
        {
            MeasurementContext.Worker.Axis_LeftSM_X,
            MeasurementContext.Worker.Axis_LeftSM_Y,
            MeasurementContext.Worker.Axis_LeftSM_Z,
            MeasurementContext.Worker.Axis_MidSM_X,
            MeasurementContext.Worker.Axis_MidSM_Y,
            MeasurementContext.Worker.Axis_MidSM_Z,
            MeasurementContext.Worker.Axis_RightSM_X,
            MeasurementContext.Worker.Axis_RightSM_Y,
        };


        MeasurementAxis[] Axis_CardC = new MeasurementAxis[]
        {
            MeasurementContext.Worker.Axis_LeftBend_DWR,
            MeasurementContext.Worker.Axis_LeftBend_DWW,
            MeasurementContext.Worker.Axis_LeftBend_stgY,
            MeasurementContext.Worker.Axis_MidBend_CCDX,
            MeasurementContext.Worker.Axis_MidBend_DWX,
            MeasurementContext.Worker.Axis_MidBend_DWY,
            MeasurementContext.Worker.Axis_MidBend_DWR,
            MeasurementContext.Worker.Axis_MidBend_DWW
        };

        MeasurementAxis[] Axis_CardD = new MeasurementAxis[]
        {
            MeasurementContext.Worker.Axis_MidBend_stgY,
            MeasurementContext.Worker.Axis_RightBend_CCDX,
            MeasurementContext.Worker.Axis_RightBend_DWX,
            MeasurementContext.Worker.Axis_RightBend_DWY,
            MeasurementContext.Worker.Axis_RightBend_DWR,
            MeasurementContext.Worker.Axis_RightBend_DWW,
            MeasurementContext.Worker.Axis_RightBend_stgY,
            MeasurementContext.Worker.Axis_Discharge_X
        };

        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.ExStyle |= 0x02000000;
                return parms;
            }
        }

        protected override void OnLoginChanged()
        {
            groupBoxEx38.Enabled = (MeasurementContext.UesrManage.LoginType != UserLevel.LoginTypes.None);
            groupBoxEx1.Enabled = (MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Engineer
                || MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Manufacturer);
            groupBoxEx75.Enabled = (MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Engineer
                || MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Manufacturer);
            cTabControl1.Enabled = (MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Manufacturer);
            cTabControl2.Enabled = (MeasurementContext.UesrManage.LoginType == UserLevel.LoginTypes.Manufacturer);
            cTabControl3.Enabled = (MeasurementContext.UesrManage.LoginType != UserLevel.LoginTypes.None);
        }



        private void FrmSet_Load(object sender, EventArgs e)
        {
            MeasurementContext.Worker.WorkStatusChanged += Worker_WorkStatusChanged;
            MeasurementContext.Worker.RecipeChanged += Worker_RecipeChanged;

        }

        private void Worker_WorkStatusChanged(object sender, EventArgs e)
        {
            if (MeasurementContext.Worker.workstatus == WorkStatuses.Running
                || MeasurementContext.Worker.workstatus == WorkStatuses.Pausing
                || MeasurementContext.Worker.workstatus == WorkStatuses.Stoping)
            {
                if (this.InvokeRequired)
                {
                    Invoke(new MethodInvoker(() => _RefreshCheckFalse()));
                }
                else
                {
                    _RefreshCheckFalse();
                }
            }
            else
            {
                if (this.InvokeRequired)
                {
                    Invoke(new MethodInvoker(() => _RefreshCheckTrue()));
                }
                else
                {
                    _RefreshCheckTrue();
                }
            }

        }


        private void _RefreshCheckFalse()
        {
            chk_leftsm_enable.Enabled = false;
            chk_midsm_enable.Enabled = false;
            chk_rightsm_enable.Enabled = false;

            chk_IsLoadZCylinder.Enabled = false;

            chk_IsTransferZCylinder.Enabled = false;

            chk_leftbend_enable.Enabled = false;
            chk__midbend_enable.Enabled = false;
            chk_rightbend_enable.Enabled = false;

            axisCardPanelC1.Enabled = false;
            axisCardPanelC2.Enabled = false;
            axisCardPanelC3.Enabled = false;
            axisCardPanelC4.Enabled = false;
            cardAIOSet1.Enabled = false;
            cardBIOSet1.Enabled = false;
            cardCIOSet1.Enabled = false;
            cardDIOSet1.Enabled = false;
            cardModulA1.Enabled = false;
            cardModulB1.Enabled = false;
            cardModulC1.Enabled = false;

            axisMovePanelEx1.Enabled = false;
            axisMovePanelEx2.Enabled = false;
            axisMovePanelEx3.Enabled = false;
            axisMovePanelEx4.Enabled = false;
            axisMovePanelEx5.Enabled = false;

            axisSetPanel1.Enabled = false;
            axisSetPanel2.Enabled = false;
            axisSetPanel3.Enabled = false;
            axisSetPanel4.Enabled = false;


        }

        private void _RefreshCheckTrue()
        {
            chk_leftsm_enable.Enabled = true;
            chk_midsm_enable.Enabled = true;
            chk_rightsm_enable.Enabled = true;

            chk_IsLoadZCylinder.Enabled = true;
            chk_IsTransferZCylinder.Enabled = true;

            chk_leftbend_enable.Enabled = true;
            chk__midbend_enable.Enabled = true;
            chk_rightbend_enable.Enabled = true;

            axisCardPanelC1.Enabled = true;
            axisCardPanelC2.Enabled = true;
            axisCardPanelC3.Enabled = true;
            axisCardPanelC4.Enabled = true;
            cardAIOSet1.Enabled = true;
            cardBIOSet1.Enabled = true;
            cardCIOSet1.Enabled = true;
            cardDIOSet1.Enabled = true;
            cardModulA1.Enabled = true;
            cardModulB1.Enabled = true;
            cardModulC1.Enabled = true;

            axisMovePanelEx1.Enabled = true;
            axisMovePanelEx2.Enabled = true;
            axisMovePanelEx3.Enabled = true;
            axisMovePanelEx4.Enabled = true;
            axisMovePanelEx5.Enabled = true;
            axisMovePanel1.Enabled = true;

            axisSetPanel1.Enabled = true;
            axisSetPanel2.Enabled = true;
            axisSetPanel3.Enabled = true;
            axisSetPanel4.Enabled = true;
        }


        private void Worker_RecipeChanged(object sender, EventArgs e)
        {
            InitBendPara();
        }


        private void Init()
        {

            axisCardPanelC1.Init(Axis_CardC);
            axisCardPanelC2.Init(Axis_CardD);
            axisCardPanelC3.Init(Axises_CardA);
            axisCardPanelC4.Init(Axises_CardB);


            axisMovePanelEx1.InitAxis(MeasurementContext.Config.LeftSM_WAxis_ServoAlarmIOInEx, MeasurementContext.Config.Left_WServoOn_IOOutEx, MeasurementContext.Worker.Axis_LeftSM_W);
            axisMovePanelEx2.InitAxis(MeasurementContext.Config.MidSM_WAxis_ServoAlarmIOInEx, MeasurementContext.Config.Mid_WServoOn_IOOutEx, MeasurementContext.Worker.Axis_MidSM_W);
            axisMovePanelEx3.InitAxis(MeasurementContext.Config.RightSM_WAxis_ServoAlarmIOInEx, MeasurementContext.Config.Right_WServoOn_IOOutEx, MeasurementContext.Worker.Axis_RightSM_W);
            axisMovePanelEx4.InitAxis(MeasurementContext.Config.DischargeAxisZservorAlarmIOInEx, MeasurementContext.Config.DischargZServoOn_IOOutEx, MeasurementContext.Worker.Axis_Discharge_Z);
            axisMovePanelEx5.InitAxis(MeasurementContext.Config.SM_CCDXAxis_Alarm_IOInEx, MeasurementContext.Config.DischargZbrak_IOOutEx, MeasurementContext.Worker.Axis_SMCCD_X);
            if (MeasurementContext.Config.IsLoadYAxisEnable)
            {
                axisMovePanel1.Enabled = true;
                axisMovePanel1.Axis = MeasurementContext.Worker.Axis_Load_Y;
            }
            else { axisMovePanel1.Enabled = false; }
                

            cardAIOSet1.Init();
            cardBIOSet1.Init();
            cardCIOSet1.Init();
            cardDIOSet1.Init();
            cardModulA1.Init();
            cardModulB1.Init();
            cardModulC1.Init();

            axisSetPanel1.Init(new MeasurementMotion[] { MeasurementContext.Worker.MotionA });
            axisSetPanel2.Init(new MeasurementMotion[] { MeasurementContext.Worker.MotionB });
            axisSetPanel3.Init(new MeasurementMotion[] { MeasurementContext.Worker.MotionC });
            axisSetPanel4.Init(new MeasurementMotion[] { MeasurementContext.Worker.MotionD });

            chk_leftsm_enable.IsCkecked = MeasurementContext.Config.IsLeftSMDisabled;
            chk_midsm_enable.IsCkecked = MeasurementContext.Config.IsMidSMDisabled;
            chk_rightsm_enable.IsCkecked = MeasurementContext.Config.IsRightSMDisabled;

            chk_leftsm_aoi_enable.IsCkecked = MeasurementContext.Config.IsLeftSMAOIDisabled;
            chk_midsm_aoi_enable.IsCkecked = MeasurementContext.Config.IsMidSMAOIDisabled;
            chk_rightsm_aoi_enable.IsCkecked = MeasurementContext.Config.IsRightSMAOIDisabled;

            chk_leftbend_enable.IsCkecked = MeasurementContext.Config.IsLeftBendDisabled;
            chk__midbend_enable.IsCkecked = MeasurementContext.Config.IsMidBendDisabled;
            chk_rightbend_enable.IsCkecked = MeasurementContext.Config.IsRightBendDisabled;

            chk_leftbend_aoi_enable.IsCkecked = MeasurementContext.Config.IsLeftBendAOIDisabled;
            chk_midbend_aoi_enable.IsCkecked = MeasurementContext.Config.IsMidBendAOIDisabled;
            chk_rightbend_aoi_enable.IsCkecked = MeasurementContext.Config.IsRightBendAOIDisabled;

            chk_Runnull.IsCkecked = MeasurementContext.Config.IsRunNull;
            chk_gatealarm_enabled.IsCkecked = MeasurementContext.Config.IsGateAlarm_Enable;
            num_FeedingBeltDelay.Value = MeasurementContext.Config.FeedBeltLineDelay;
            num_DischargeBeltDelay.Value = MeasurementContext.Config.DischargeBeltDelay;

            Inp_LeftPressure.Value = MeasurementContext.Config.LeftBendPressure;
            Inp_midPressure.Value = MeasurementContext.Config.MidBendPressure;
            Inp_rightPressure.Value = MeasurementContext.Config.RightBendPressure;

            chk_leftsm_sm_enable.IsCkecked = MeasurementContext.Config.IsLeftSM_SM_Enable;
            chk_midsm_sm_enable.IsCkecked = MeasurementContext.Config.IsMidSM_SM_Enable;
            chk_rightsm_sm_enable.IsCkecked = MeasurementContext.Config.IsRightSM_SM_Enable;


            chk_tear1RllCLD_Enable.IsCkecked = MeasurementContext.Config.Tear1RllCLD_Enable;
            chk_tear2RllCLD_Enable.IsCkecked = MeasurementContext.Config.Tear2RllCLD_Enable;
            chk_tear3RllCLD_Enable.IsCkecked = MeasurementContext.Config.Tear3RllCLD_Enable;



            num_RobotDropDelay.Value = MeasurementContext.Config.RobotDropDelay;
            num_RobotFetchDelay.Value = MeasurementContext.Config.RobotFetchDelay;
            num_RobotBlowDelay.Value = MeasurementContext.Config.RobotBlowDelay;
            num_StageBlowDelay.Value = MeasurementContext.Config.StageBlowDelay;
            num_TearRecheckCountParam.Value = MeasurementContext.Config.TearRecheckCountParam;
            num_LoadCellRstInterval.Value = MeasurementContext.Config.LoadCellTestInterval;

            num_AdjustAngle.Value = MeasurementContext.Config.AdjustAngle;
            num_SMUseMax.Value = MeasurementContext.Config.SMUseMax;


            chk_Glue1_Enable.IsCkecked = MeasurementContext.Config.Glue1_Enable;
            chk_Glue2_Enable.IsCkecked = MeasurementContext.Config.Glue2_Enable;
            chk_Glue3_Enable.IsCkecked = MeasurementContext.Config.Glue3_Enable;



            chk_optical1_Enable.IsCkecked = MeasurementContext.Config.FPC1_Optical_Enable;
            chk_optical2_Enable.IsCkecked = MeasurementContext.Config.FPC2_Optical_Enable;
            chk_optical3_Enable.IsCkecked = MeasurementContext.Config.FPC3_Optical_Enable;

            chk_bend1Free.IsCkecked = MeasurementContext.Config.IsBend1Free;
            chk_bend2Free.IsCkecked = MeasurementContext.Config.IsBend2Free;
            Chk_Bend3Free.IsCkecked = MeasurementContext.Config.IsBend3Free;

            chk_loadcell1_Enable.IsCkecked = MeasurementContext.Config.IsLoadCell1Enable;
            chk_loadcell2_Enable.IsCkecked = MeasurementContext.Config.IsLoadCell2Enable;
            chk_loadcell3_Enable.IsCkecked = MeasurementContext.Config.IsLoadCell3Enable;
            chk_loadcellRstEnable.IsCkecked = MeasurementContext.Config.IsLoadCellRstEnable;
            chk_ManualLoad.IsCkecked = MeasurementContext.Config.IsManualLoad;
            



            chk_QRCode_Enable.IsCkecked = MeasurementContext.Config.IsScanCodeEnable;
            checkButton1.IsCkecked = MeasurementContext.Config.FPC_Tear_Enable;
            chk_tearAOIBlow.IsCkecked = MeasurementContext.Config.TearAOI_Blow_Enable;
            chk_dsgFullSensorEnable.IsCkecked = MeasurementContext.Config.DsgLine_FullSensor_Enable;

            chk_TearRecheck_Enabled.IsCkecked = MeasurementContext.Config.TearRecheckEnabled;

            chk_FeedCylinder_Enable.IsCkecked = MeasurementContext.Config.IsFeedCylinderEnable;
            chk_DischargeCylinder_Enable.IsCkecked = MeasurementContext.Config.IsDischargeCylinderEnable;

            num_WeighMeasureDelay.Value = MeasurementContext.Config.WeighMeasureDelay;
            num_WeighResetDelay.Value = MeasurementContext.Config.WeighResetDelay;
            num_WeighValueScale.Value = MeasurementContext.Config.WeighValueScale;



            //----------------------------------------------------------------------------------

            chk_NGNotBendOutType.IsCkecked = MeasurementContext.Config.NGNotBendOutType;
            txb_printpath.Text = MeasurementContext.Config.PrintPath;
            chk_DischargeAxiaZCylinder_Enable.IsCkecked = MeasurementContext.Config.DischargeAxiaZCylinderEnable;
            chk_ControlUpStreamEnable.IsCkecked = MeasurementContext.Config.IsControlUpStreamEnable;

            chk_BefoTearCheck.IsCkecked = MeasurementContext.Config.BefoTearCheck;

            //--------------------------------------[对位保护]---------------------------------
            chk_bend1_CalibProtect.IsCkecked = MeasurementContext.Config.Isbend1_CalibProtect;
            chk_bend2_CalibProtect.IsCkecked = MeasurementContext.Config.Isbend2_CalibProtect;
            chk_bend3_CalibProtect.IsCkecked = MeasurementContext.Config.Isbend3_CalibProtect;


            //--------------------------------------[背光拍照延时]-----------------------------

            Inp_LeftBLPhotoDelay.Value = MeasurementContext.Config.LeftBLPhotoDelay;
            Inp_MidBLPhotoDelay.Value = MeasurementContext.Config.MidBLPhotoDelay;
            Inp_RightBLPhotoDelay.Value = MeasurementContext.Config.RightBLPhotoDelay;


            //-------------------------------------[翻转R轴延时]----------------------------------
            Inp_LeftRotateMoveDelay.Value = MeasurementContext.Config.LeftRotateMoveDelay;
            Inp_MidRotateMoveDelay.Value = MeasurementContext.Config.MidRotateMoveDelay;
            Inp_RightRotateMoveDelay.Value = MeasurementContext.Config.RightRotateMoveDelay;


            //-------------------------------------[回原偏移]-------------------------------------

            Inp_LoadXGoHomeOffset.Value = MeasurementContext.Config.LoadXGoHomeOffset;
            Inp_DischargeZGoHomeOffset.Value = MeasurementContext.Config.DischargeZGoHomeOffset;
            num_DiscargeZUpSpeed.Value = MeasurementContext.Config.DiscargeZUpSpeed;



            chk_DischargeZMonitor.IsCkecked = MeasurementContext.Config.IsDischargeZMonitor;
            num_DiscargeXNGUnLoadSpeed.Value = MeasurementContext.Config.DiscargeXNGUnLoadSpeed;
            num_LoadFilterTime.Value = MeasurementContext.Config.LoadFilterTime;
            num_LoadYBackSpeed.Value = MeasurementContext.Config.LoadYBackSpeed;

            chk_NGLineButton.IsCkecked = MeasurementContext.Config.IsUseNGLineButton;
            chk_BendXYHomeEnable.IsCkecked = MeasurementContext.Config.IsBendXYHomeEnable;

            chk_DischargeZHome.IsCkecked = MeasurementContext.Config.DischargeZHomeEnable;


            chk_IsLoadZCylinder.IsCkecked = MeasurementContext.Config.IsLoadZCylinder;
            num_XYHomeInterval.Value = MeasurementContext.Config.YHomeInterval;
            num_ZHomeInterval.Value = MeasurementContext.Config.ZHomeInterval;

            chk_IsTransferZCylinder.IsCkecked = MeasurementContext.Config.IsTransferZCylinder;


            chk_TearFilmCloseVac.IsCkecked = MeasurementContext.Config.IsTearFilmCloseVacCalib;
            chk_PreTearFilmPress.IsCkecked = MeasurementContext.Config.IsPreTearFilmPress;

            //-------------------------------------[ZGH20220912新增]----------------------------------
            chk_LoadYAxis.IsCkecked = MeasurementContext.Config.IsLoadYAxisEnable;

            //↓撕膜平台上下气缸功能
            chk_LeftSMUDCylinder.IsCkecked = MeasurementContext.Config.IsLeftSMUDCylinderEnable;
            chk_MidSMUDCylinder.IsCkecked = MeasurementContext.Config.IsMidSMUDCylinderEnable;
            chk_RightUDCylinder.IsCkecked = MeasurementContext.Config.IsRightUDCylinderEnable;
            //↓撕膜气缸对位延时
            num_LeftSMCylinderAligningDelay.Value = MeasurementContext.Config.LeftSMCylinderAligningDelay;
            num_MidSMCylinderAligningDelay.Value = MeasurementContext.Config.MidSMCylinderAligningDelay;
            num_RightSMCylinderAligningDelay.Value = MeasurementContext.Config.RightSMCylinderAligningDelay;
            //----------------------------------------------------------------------------------------
        }


        private void UpdateCHK()
        {
            MeasurementContext.Config.IsPreTearFilmPress = chk_PreTearFilmPress.IsCkecked;
            MeasurementContext.Config.IsTearFilmCloseVacCalib = chk_TearFilmCloseVac.IsCkecked;
            MeasurementContext.Config.YHomeInterval = (int)num_XYHomeInterval.Value;
            MeasurementContext.Config.ZHomeInterval = (int)num_ZHomeInterval.Value;
            MeasurementContext.Config.IsLoadZCylinder = chk_IsLoadZCylinder.IsCkecked;
            MeasurementContext.Config.IsTransferZCylinder = chk_IsTransferZCylinder.IsCkecked;
            MeasurementContext.Config.IsBendXYHomeEnable = chk_BendXYHomeEnable.IsCkecked;
            MeasurementContext.Config.DischargeZHomeEnable = chk_DischargeZHome.IsCkecked;
            MeasurementContext.Config.IsUseNGLineButton = chk_NGLineButton.IsCkecked;
            MeasurementContext.Config.LoadYBackSpeed = (int)num_LoadYBackSpeed.Value;
            if (MeasurementContext.Config.LoadYBackSpeed > (int)num_LoadYBackSpeed.MaxValue)
            {
               
                num_LoadYBackSpeed.Value = num_LoadYBackSpeed.MaxValue;
                MeasurementContext.Config.LoadYBackSpeed = (int)num_LoadYBackSpeed.Value;
            }


            MeasurementContext.Config.LoadFilterTime = (int)num_LoadFilterTime.Value;
            MeasurementContext.Config.DiscargeXNGUnLoadSpeed = (int)num_DiscargeXNGUnLoadSpeed.Value;
            if (MeasurementContext.Config.DiscargeXNGUnLoadSpeed > (int)num_DiscargeXNGUnLoadSpeed.MaxValue)
            {
                num_DiscargeXNGUnLoadSpeed.Value = num_DiscargeXNGUnLoadSpeed.MaxValue;
                MeasurementContext.Config.DiscargeXNGUnLoadSpeed = (int)num_DiscargeXNGUnLoadSpeed.Value;
            }

            MeasurementContext.Config.IsDischargeZMonitor = chk_DischargeZMonitor.IsCkecked;

            //-------------------------------------[↓回原偏移]----------------------------------
            MeasurementContext.Config.LoadXGoHomeOffset = Inp_LoadXGoHomeOffset.Value;
            MeasurementContext.Config.DischargeZGoHomeOffset = Inp_DischargeZGoHomeOffset.Value;
            MeasurementContext.Config.DiscargeZUpSpeed = (int)num_DiscargeZUpSpeed.Value;
            if (MeasurementContext.Config.DiscargeZUpSpeed > (int)num_DiscargeZUpSpeed.MaxValue)
            {
                num_DiscargeZUpSpeed.Value = num_DiscargeZUpSpeed.MaxValue;
                MeasurementContext.Config.DiscargeZUpSpeed = (int)num_DiscargeZUpSpeed.MaxValue;
            }

            //-------------------------------------[↓翻转R轴延时]----------------------------------
            MeasurementContext.Config.LeftRotateMoveDelay = (int)Inp_LeftRotateMoveDelay.Value;
            MeasurementContext.Config.MidRotateMoveDelay = (int)Inp_MidRotateMoveDelay.Value;
            MeasurementContext.Config.RightRotateMoveDelay = (int)Inp_RightRotateMoveDelay.Value;

            //--------------------------------------[↓背光拍照延时]---------------------------
            MeasurementContext.Config.LeftBLPhotoDelay = (int)Inp_LeftBLPhotoDelay.Value;
            MeasurementContext.Config.MidBLPhotoDelay = (int)Inp_MidBLPhotoDelay.Value;
            MeasurementContext.Config.RightBLPhotoDelay = (int)Inp_RightBLPhotoDelay.Value;

            //--------------------------------------[↓对位保护]-------------------------------
            MeasurementContext.Config.BefoTearCheck = chk_BefoTearCheck.IsCkecked;
            MeasurementContext.Config.IsControlUpStreamEnable = chk_ControlUpStreamEnable.IsCkecked;
            MeasurementContext.Config.DischargeAxiaZCylinderEnable = chk_DischargeAxiaZCylinder_Enable.IsCkecked;
            MeasurementContext.Config.PrintPath = txb_printpath.Text;
            MeasurementContext.Config.NGNotBendOutType = chk_NGNotBendOutType.IsCkecked;



            //--------------------------------------[对位保护]--------------------------------
            MeasurementContext.Config.Isbend1_CalibProtect = chk_bend1_CalibProtect.IsCkecked;
            MeasurementContext.Config.Isbend2_CalibProtect = chk_bend2_CalibProtect.IsCkecked;
            MeasurementContext.Config.Isbend3_CalibProtect = chk_bend3_CalibProtect.IsCkecked;

            MeasurementContext.Config.WeighMeasureDelay = (int)num_WeighMeasureDelay.Value;
            MeasurementContext.Config.WeighResetDelay = (int)num_WeighResetDelay.Value;
            MeasurementContext.Config.WeighValueScale = num_WeighValueScale.Value;

            MeasurementContext.Config.IsFeedCylinderEnable = chk_FeedCylinder_Enable.IsCkecked;
            MeasurementContext.Config.IsDischargeCylinderEnable = chk_DischargeCylinder_Enable.IsCkecked;

            MeasurementContext.Config.TearRecheckEnabled = chk_TearRecheck_Enabled.IsCkecked;
            MeasurementContext.Config.IsLeftSMDisabled = chk_leftsm_enable.IsCkecked;
            MeasurementContext.Config.IsMidSMDisabled = chk_midsm_enable.IsCkecked;
            MeasurementContext.Config.IsRightSMDisabled = chk_rightsm_enable.IsCkecked;

            MeasurementContext.Config.IsLeftSMAOIDisabled = chk_leftsm_aoi_enable.IsCkecked;
            MeasurementContext.Config.IsMidSMAOIDisabled = chk_midsm_aoi_enable.IsCkecked;
            MeasurementContext.Config.IsRightSMAOIDisabled = chk_rightsm_aoi_enable.IsCkecked;

            MeasurementContext.Config.IsLeftBendDisabled = chk_leftbend_enable.IsCkecked;
            MeasurementContext.Config.IsMidBendDisabled = chk__midbend_enable.IsCkecked;
            MeasurementContext.Config.IsRightBendDisabled = chk_rightbend_enable.IsCkecked;

            MeasurementContext.Config.IsLeftBendAOIDisabled = chk_leftbend_aoi_enable.IsCkecked;
            MeasurementContext.Config.IsMidBendAOIDisabled = chk_midbend_aoi_enable.IsCkecked;
            MeasurementContext.Config.IsRightBendAOIDisabled = chk_rightbend_aoi_enable.IsCkecked;

            MeasurementContext.Config.IsRunNull = chk_Runnull.IsCkecked;
            MeasurementContext.Config.IsGateAlarm_Enable = chk_gatealarm_enabled.IsCkecked;
            MeasurementContext.Config.FeedBeltLineDelay = num_FeedingBeltDelay.Value;
            MeasurementContext.Config.DischargeBeltDelay = num_DischargeBeltDelay.Value;

            MeasurementContext.Config.LeftBendPressure = Inp_LeftPressure.Value;
            MeasurementContext.Config.MidBendPressure = Inp_midPressure.Value;
            MeasurementContext.Config.RightBendPressure = Inp_rightPressure.Value;

            MeasurementContext.Config.IsLeftSM_SM_Enable = chk_leftsm_sm_enable.IsCkecked;
            MeasurementContext.Config.IsMidSM_SM_Enable = chk_midsm_sm_enable.IsCkecked;
            MeasurementContext.Config.IsRightSM_SM_Enable = chk_rightsm_sm_enable.IsCkecked;

            MeasurementContext.Config.Tear1RllCLD_Enable = chk_tear1RllCLD_Enable.IsCkecked;
            MeasurementContext.Config.Tear2RllCLD_Enable = chk_tear2RllCLD_Enable.IsCkecked;
            MeasurementContext.Config.Tear3RllCLD_Enable = chk_tear3RllCLD_Enable.IsCkecked;



            MeasurementContext.Config.RobotDropDelay = (int)num_RobotDropDelay.Value;
            MeasurementContext.Config.RobotFetchDelay = (int)num_RobotFetchDelay.Value;
            MeasurementContext.Config.StageBlowDelay = (int)num_StageBlowDelay.Value;
            MeasurementContext.Config.RobotBlowDelay = (int)num_RobotBlowDelay.Value;
            MeasurementContext.Config.TearRecheckCountParam = (int)num_TearRecheckCountParam.Value;

            MeasurementContext.Config.LoadCellTestInterval = (int)num_LoadCellRstInterval.Value;
            MeasurementContext.Config.AdjustAngle = (float)num_AdjustAngle.Value;
            MeasurementContext.Config.SMUseMax = (int)num_SMUseMax.Value;

            MeasurementContext.Config.Glue1_Enable = chk_Glue1_Enable.IsCkecked;
            MeasurementContext.Config.Glue2_Enable = chk_Glue2_Enable.IsCkecked;
            MeasurementContext.Config.Glue3_Enable = chk_Glue3_Enable.IsCkecked;



            MeasurementContext.Config.FPC1_Optical_Enable = chk_optical1_Enable.IsCkecked;
            MeasurementContext.Config.FPC2_Optical_Enable = chk_optical2_Enable.IsCkecked;
            MeasurementContext.Config.FPC3_Optical_Enable = chk_optical3_Enable.IsCkecked;

            MeasurementContext.Config.IsBend1Free = chk_bend1Free.IsCkecked;
            MeasurementContext.Config.IsBend2Free = chk_bend2Free.IsCkecked;
            MeasurementContext.Config.IsBend3Free = Chk_Bend3Free.IsCkecked;

            MeasurementContext.Config.IsLoadCell1Enable = chk_loadcell1_Enable.IsCkecked;
            MeasurementContext.Config.IsLoadCell2Enable = chk_loadcell2_Enable.IsCkecked;
            MeasurementContext.Config.IsLoadCell3Enable = chk_loadcell3_Enable.IsCkecked;
            MeasurementContext.Config.IsLoadCellRstEnable = chk_loadcellRstEnable.IsCkecked;
            MeasurementContext.Config.IsManualLoad = chk_ManualLoad.IsCkecked;



            MeasurementContext.Config.IsScanCodeEnable = chk_QRCode_Enable.IsCkecked;
            MeasurementContext.Config.FPC_Tear_Enable = checkButton1.IsCkecked;
            MeasurementContext.Config.TearAOI_Blow_Enable = chk_tearAOIBlow.IsCkecked;
            MeasurementContext.Config.DsgLine_FullSensor_Enable = chk_dsgFullSensorEnable.IsCkecked;

            //-------------------------------------[ZGH20220912新增]----------------------------------
            MeasurementContext.Config.IsLoadYAxisEnable = chk_LoadYAxis.IsCkecked;

            //↓撕膜平台上下气缸功能
            MeasurementContext.Config.IsLeftSMUDCylinderEnable = chk_LeftSMUDCylinder.IsCkecked;
            MeasurementContext.Config.IsMidSMUDCylinderEnable = chk_MidSMUDCylinder.IsCkecked;
            MeasurementContext.Config.IsRightUDCylinderEnable = chk_RightUDCylinder.IsCkecked;
            //↓撕膜气缸对位延时
            MeasurementContext.Config.LeftSMCylinderAligningDelay = (int)num_LeftSMCylinderAligningDelay.Value;
            MeasurementContext.Config.MidSMCylinderAligningDelay = (int)num_MidSMCylinderAligningDelay.Value;
            MeasurementContext.Config.RightSMCylinderAligningDelay = (int)num_RightSMCylinderAligningDelay.Value;
            //----------------------------------------------------------------------------------------
        }

        private void InitBendPara()
        {
            recipe = MeasurementContext.Data.CurrentRecipeData;
            if (recipe != null)
            {






                Var_AOIX1.Value = recipe.AOIX1;
                Var_AOIX2.Value = recipe.AOIX2;
                Var_AOIY1.Value = recipe.AOIY1;
                Var_AOIY2.Value = recipe.AOIY2;

                Var_AOIX1Offset.Value = recipe.AOIX1Offset;
                Var_AOIX2Offset.Value = recipe.AOIX2Offset;
                Var_AOIY1Offset.Value = recipe.AOIY1Offset;
                Var_AOIY2Offset.Value = recipe.AOIY2Offset;


                Var_MidAOIX1.Value = recipe.MidAOIX1;
                Var_MidAOIX2.Value = recipe.MidAOIX2;
                Var_MidAOIY1.Value = recipe.MidAOIY1;
                Var_MidAOIY2.Value = recipe.MidAOIY2;


                Var_MidAOIX1Offset.Value = recipe.MidAOIX1Offset;
                Var_MidAOIX2Offset.Value = recipe.MidAOIX2Offset;
                Var_MidAOIY1Offset.Value = recipe.MidAOIY1Offset;
                Var_MidAOIY2Offset.Value = recipe.MidAOIY2Offset;


                Var_RightAOIX1.Value = recipe.RightAOIX1;
                Var_RightAOIX2.Value = recipe.RightAOIX2;
                Var_RightAOIY1.Value = recipe.RightAOIY1;
                Var_RightAOIY2.Value = recipe.RightAOIY2;

                Var_RightAOIX1Offset.Value = recipe.RightAOIX1Offset;
                Var_RightAOIX2Offset.Value = recipe.RightAOIX2Offset;
                Var_RightAOIY1Offset.Value = recipe.RightAOIY1Offset;
                Var_RightAOIY2Offset.Value = recipe.RightAOIY2Offset;







                Inp_LoadCell1Value.Value = recipe.LoadCell1Limit;
                Inp_LoadCell2Value.Value = recipe.LoadCell2Limit;
                Inp_LoadCell3Value.Value = recipe.LoadCell3Limit;







                Var_LeftY1_BaseRate.Value = recipe.BendPara[0].BaseRate;
                Var_LeftY1_Errand.Value = recipe.BendPara[0].ErrAnd;
                Var_LeftY1_Low1.Value = recipe.BendPara[0].Zone1Low;
                Var_LeftY1_Up1.Value = recipe.BendPara[0].Zone1Up;
                Var_LeftY1_Low2.Value = recipe.BendPara[0].Zone2Low;
                Var_LeftY1_Up2.Value = recipe.BendPara[0].Zone2Up;
                Var_LeftY1_Low3.Value = recipe.BendPara[0].Zone3Low;
                Var_LeftY1_Up3.Value = recipe.BendPara[0].Zone3Up;
                Var_LeftY1_Rate1.Value = recipe.BendPara[0].Rate1;
                Var_LeftY1_Rate2.Value = recipe.BendPara[0].Rate2;
                Var_LeftY1_Rate3.Value = recipe.BendPara[0].Rate3;
                Var_LeftY1_Num.Value = recipe.BendPara[0].Adj_Num;


                Var_LeftY2_BaseRate.Value = recipe.BendPara[1].BaseRate;
                Var_LeftY2_Errand.Value = recipe.BendPara[1].ErrAnd;
                Var_LeftY2_Low1.Value = recipe.BendPara[1].Zone1Low;
                Var_LeftY2_Up1.Value = recipe.BendPara[1].Zone1Up;
                Var_LeftY2_Low2.Value = recipe.BendPara[1].Zone2Low;
                Var_LeftY2_Up2.Value = recipe.BendPara[1].Zone2Up;
                Var_LeftY2_Low3.Value = recipe.BendPara[1].Zone3Low;
                Var_LeftY2_Up3.Value = recipe.BendPara[1].Zone3Up;
                Var_LeftY2_Rate1.Value = recipe.BendPara[1].Rate1;
                Var_LeftY2_Rate2.Value = recipe.BendPara[1].Rate2;
                Var_LeftY2_Rate3.Value = recipe.BendPara[1].Rate3;
                Var_LeftY2_Num.Value = recipe.BendPara[1].Adj_Num;

                Var_MidY1_BaseRate.Value = recipe.BendPara[2].BaseRate;
                Var_MidY1_Errand.Value = recipe.BendPara[2].ErrAnd;
                Var_MidY1_Low1.Value = recipe.BendPara[2].Zone1Low;
                Var_MidY1_Up1.Value = recipe.BendPara[2].Zone1Up;
                Var_MidY1_Low2.Value = recipe.BendPara[2].Zone2Low;
                Var_MidY1_Up2.Value = recipe.BendPara[2].Zone2Up;
                Var_MidY1_Low3.Value = recipe.BendPara[2].Zone3Low;
                Var_MidY1_Up3.Value = recipe.BendPara[2].Zone3Up;
                Var_MidY1_Rate1.Value = recipe.BendPara[2].Rate1;
                Var_MidY1_Rate2.Value = recipe.BendPara[2].Rate2;
                Var_MidY1_Rate3.Value = recipe.BendPara[2].Rate3;
                Var_MidY1_Num.Value = recipe.BendPara[2].Adj_Num;

                Var_MidY2_BaseRate.Value = recipe.BendPara[3].BaseRate;
                Var_MidY2_Errand.Value = recipe.BendPara[3].ErrAnd;
                Var_MidY2_Low1.Value = recipe.BendPara[3].Zone1Low;
                Var_MidY2_Up1.Value = recipe.BendPara[3].Zone1Up;
                Var_MidY2_Low2.Value = recipe.BendPara[3].Zone2Low;
                Var_MidY2_Up2.Value = recipe.BendPara[3].Zone2Up;
                Var_MidY2_Low3.Value = recipe.BendPara[3].Zone3Low;
                Var_MidY2_Up3.Value = recipe.BendPara[3].Zone3Up;
                Var_MidY2_Rate1.Value = recipe.BendPara[3].Rate1;
                Var_MidY2_Rate2.Value = recipe.BendPara[3].Rate2;
                Var_MidY2_Rate3.Value = recipe.BendPara[3].Rate3;
                Var_MidY2_Num.Value = recipe.BendPara[3].Adj_Num;

                Var_RightY1_BaseRate.Value = recipe.BendPara[4].BaseRate;
                Var_RightY1_Errand.Value = recipe.BendPara[4].ErrAnd;
                Var_RightY1_Low1.Value = recipe.BendPara[4].Zone1Low;
                Var_RightY1_Up1.Value = recipe.BendPara[4].Zone1Up;
                Var_RightY1_Low2.Value = recipe.BendPara[4].Zone2Low;
                Var_RightY1_Up2.Value = recipe.BendPara[4].Zone2Up;
                Var_RightY1_Low3.Value = recipe.BendPara[4].Zone3Low;
                Var_RightY1_Up3.Value = recipe.BendPara[4].Zone3Up;
                Var_RightY1_Rate1.Value = recipe.BendPara[4].Rate1;
                Var_RightY1_Rate2.Value = recipe.BendPara[4].Rate2;
                Var_RightY1_Rate3.Value = recipe.BendPara[4].Rate3;
                Var_RightY1_Num.Value = recipe.BendPara[4].Adj_Num;

                Var_RightY2_BaseRate.Value = recipe.BendPara[5].BaseRate;
                Var_RightY2_Errand.Value = recipe.BendPara[5].ErrAnd;
                Var_RightY2_Low1.Value = recipe.BendPara[5].Zone1Low;
                Var_RightY2_Up1.Value = recipe.BendPara[5].Zone1Up;
                Var_RightY2_Low2.Value = recipe.BendPara[5].Zone2Low;
                Var_RightY2_Up2.Value = recipe.BendPara[5].Zone2Up;
                Var_RightY2_Low3.Value = recipe.BendPara[5].Zone3Low;
                Var_RightY2_Up3.Value = recipe.BendPara[5].Zone3Up;
                Var_RightY2_Rate1.Value = recipe.BendPara[5].Rate1;
                Var_RightY2_Rate2.Value = recipe.BendPara[5].Rate2;
                Var_RightY2_Rate3.Value = recipe.BendPara[5].Rate3;
                Var_RightY2_Num.Value = recipe.BendPara[5].Adj_Num;

                Check_LeftY1_DIR.IsCkecked = recipe.BendPara[0].DirValue == 1 ? true : false;
                Check_LeftY2_DIR.IsCkecked = recipe.BendPara[1].DirValue == 1 ? true : false;
                Check_MidY1_DIR.IsCkecked = recipe.BendPara[2].DirValue == 1 ? true : false;
                Check_MidY2_DIR.IsCkecked = recipe.BendPara[3].DirValue == 1 ? true : false;
                Check_RightY1_DIR.IsCkecked = recipe.BendPara[4].DirValue == 1 ? true : false;
                Check_RightY2_DIR.IsCkecked = recipe.BendPara[5].DirValue == 1 ? true : false;

                var_LeftY1_AOI.Value = recipe.BendPara[0].AOIOffset;
                var_LeftY2_AOI.Value = recipe.BendPara[1].AOIOffset;
                var_MidY1_AOI.Value = recipe.BendPara[2].AOIOffset;
                var_MidY2_AOI.Value = recipe.BendPara[3].AOIOffset;
                var_RightY1_AOI.Value = recipe.BendPara[4].AOIOffset;
                var_RightY2_AOI.Value = recipe.BendPara[5].AOIOffset;

                var_LeftY1_AOI_Base.Value = recipe.BendPara[0].AOIBase;
                var_LeftY2_AOI_Base.Value = recipe.BendPara[1].AOIBase;
                var_MidY1_AOI_Base.Value = recipe.BendPara[2].AOIBase;
                var_MidY2_AOI_Base.Value = recipe.BendPara[3].AOIBase;
                var_RightY1_AOI_Base.Value = recipe.BendPara[4].AOIBase;
                var_RightY2_AOI_Base.Value = recipe.BendPara[5].AOIBase;

                rbt_XY.Checked = recipe.WorkModel == 0 ? true : false;
                rbt_YX.Checked = recipe.WorkModel == 1 ? true : false;
                rbt_YY.Checked = recipe.WorkModel == 2 ? true : false;


                check_WorkModel.IsCkecked = recipe.XY_Model;


            }

        }

        private void UpdateRecipeValue()
        {
            recipe.AOIX1 = Var_AOIX1.Value;
            recipe.AOIX2 = Var_AOIX2.Value;
            recipe.AOIY1 = Var_AOIY1.Value;
            recipe.AOIY2 = Var_AOIY2.Value;
            recipe.AOIX1Offset = Var_AOIX1Offset.Value;
            recipe.AOIX2Offset = Var_AOIX2Offset.Value;
            recipe.AOIY1Offset = Var_AOIY1Offset.Value;
            recipe.AOIY2Offset = Var_AOIY2Offset.Value;

            recipe.MidAOIX1 = Var_MidAOIX1.Value;
            recipe.MidAOIX2 = Var_MidAOIX2.Value;
            recipe.MidAOIY1 = Var_MidAOIY1.Value;
            recipe.MidAOIY2 = Var_MidAOIY2.Value;
            recipe.MidAOIX1Offset = Var_MidAOIX1Offset.Value;
            recipe.MidAOIX2Offset = Var_MidAOIX2Offset.Value;
            recipe.MidAOIY1Offset = Var_MidAOIY1Offset.Value;
            recipe.MidAOIY2Offset = Var_MidAOIY2Offset.Value;

            recipe.RightAOIX1 = Var_RightAOIX1.Value;
            recipe.RightAOIX2 = Var_RightAOIX2.Value;
            recipe.RightAOIY1 = Var_RightAOIY1.Value;
            recipe.RightAOIY2 = Var_RightAOIY2.Value;
            recipe.RightAOIX1Offset = Var_RightAOIX1Offset.Value;
            recipe.RightAOIX2Offset = Var_RightAOIX2Offset.Value;
            recipe.RightAOIY1Offset = Var_RightAOIY1Offset.Value;
            recipe.RightAOIY2Offset = Var_RightAOIY2Offset.Value;


            recipe.LoadCell1Limit = Inp_LoadCell1Value.Value;
            recipe.LoadCell2Limit = Inp_LoadCell2Value.Value;
            recipe.LoadCell3Limit = Inp_LoadCell3Value.Value;

            recipe.BendPara[0].BaseRate = Var_LeftY1_BaseRate.Value;
            recipe.BendPara[0].ErrAnd = Var_LeftY1_Errand.Value;
            recipe.BendPara[0].Zone1Low = Var_LeftY1_Low1.Value;
            recipe.BendPara[0].Zone1Up = Var_LeftY1_Up1.Value;
            recipe.BendPara[0].Zone2Low = Var_LeftY1_Low2.Value;
            recipe.BendPara[0].Zone2Up = Var_LeftY1_Up2.Value;
            recipe.BendPara[0].Zone3Low = Var_LeftY1_Low3.Value;
            recipe.BendPara[0].Zone3Up = Var_LeftY1_Up3.Value;
            recipe.BendPara[0].Rate1 = Var_LeftY1_Rate1.Value;
            recipe.BendPara[0].Rate2 = Var_LeftY1_Rate2.Value;
            recipe.BendPara[0].Rate3 = Var_LeftY1_Rate3.Value;
            recipe.BendPara[0].Adj_Num = Var_LeftY1_Num.Value;

            recipe.BendPara[1].BaseRate = Var_LeftY2_BaseRate.Value;
            recipe.BendPara[1].ErrAnd = Var_LeftY2_Errand.Value;
            recipe.BendPara[1].Zone1Low = Var_LeftY2_Low1.Value;
            recipe.BendPara[1].Zone1Up = Var_LeftY2_Up1.Value;
            recipe.BendPara[1].Zone2Low = Var_LeftY2_Low2.Value;
            recipe.BendPara[1].Zone2Up = Var_LeftY2_Up2.Value;
            recipe.BendPara[1].Zone3Low = Var_LeftY2_Low3.Value;
            recipe.BendPara[1].Zone3Up = Var_LeftY2_Up3.Value;
            recipe.BendPara[1].Rate1 = Var_LeftY2_Rate1.Value;
            recipe.BendPara[1].Rate2 = Var_LeftY2_Rate2.Value;
            recipe.BendPara[1].Rate3 = Var_LeftY2_Rate3.Value;
            recipe.BendPara[1].Adj_Num = Var_LeftY2_Num.Value;

            recipe.BendPara[2].BaseRate = Var_MidY1_BaseRate.Value;
            recipe.BendPara[2].ErrAnd = Var_MidY1_Errand.Value;
            recipe.BendPara[2].Zone1Low = Var_MidY1_Low1.Value;
            recipe.BendPara[2].Zone1Up = Var_MidY1_Up1.Value;
            recipe.BendPara[2].Zone2Low = Var_MidY1_Low2.Value;
            recipe.BendPara[2].Zone2Up = Var_MidY1_Up2.Value;
            recipe.BendPara[2].Zone3Low = Var_MidY1_Low3.Value;
            recipe.BendPara[2].Zone3Up = Var_MidY1_Up3.Value;
            recipe.BendPara[2].Rate1 = Var_MidY1_Rate1.Value;
            recipe.BendPara[2].Rate2 = Var_MidY1_Rate2.Value;
            recipe.BendPara[2].Rate3 = Var_MidY1_Rate3.Value;
            recipe.BendPara[2].Adj_Num = Var_MidY1_Num.Value;

            recipe.BendPara[3].BaseRate = Var_MidY2_BaseRate.Value;
            recipe.BendPara[3].ErrAnd = Var_MidY2_Errand.Value;
            recipe.BendPara[3].Zone1Low = Var_MidY2_Low1.Value;
            recipe.BendPara[3].Zone1Up = Var_MidY2_Up1.Value;
            recipe.BendPara[3].Zone2Low = Var_MidY2_Low2.Value;
            recipe.BendPara[3].Zone2Up = Var_MidY2_Up2.Value;
            recipe.BendPara[3].Zone3Low = Var_MidY2_Low3.Value;
            recipe.BendPara[3].Zone3Up = Var_MidY2_Up3.Value;
            recipe.BendPara[3].Rate1 = Var_MidY2_Rate1.Value;
            recipe.BendPara[3].Rate2 = Var_MidY2_Rate2.Value;
            recipe.BendPara[3].Rate3 = Var_MidY2_Rate3.Value;
            recipe.BendPara[3].Adj_Num = Var_MidY2_Num.Value;

            recipe.BendPara[4].BaseRate = Var_RightY1_BaseRate.Value;
            recipe.BendPara[4].ErrAnd = Var_RightY1_Errand.Value;
            recipe.BendPara[4].Zone1Low = Var_RightY1_Low1.Value;
            recipe.BendPara[4].Zone1Up = Var_RightY1_Up1.Value;
            recipe.BendPara[4].Zone2Low = Var_RightY1_Low2.Value;
            recipe.BendPara[4].Zone2Up = Var_RightY1_Up2.Value;
            recipe.BendPara[4].Zone3Low = Var_RightY1_Low3.Value;
            recipe.BendPara[4].Zone3Up = Var_RightY1_Up3.Value;
            recipe.BendPara[4].Rate1 = Var_RightY1_Rate1.Value;
            recipe.BendPara[4].Rate2 = Var_RightY1_Rate2.Value;
            recipe.BendPara[4].Rate3 = Var_RightY1_Rate3.Value;
            recipe.BendPara[4].Adj_Num = Var_RightY1_Num.Value;

            recipe.BendPara[5].BaseRate = Var_RightY2_BaseRate.Value;
            recipe.BendPara[5].ErrAnd = Var_RightY2_Errand.Value;
            recipe.BendPara[5].Zone1Low = Var_RightY2_Low1.Value;
            recipe.BendPara[5].Zone1Up = Var_RightY2_Up1.Value;
            recipe.BendPara[5].Zone2Low = Var_RightY2_Low2.Value;
            recipe.BendPara[5].Zone2Up = Var_RightY2_Up2.Value;
            recipe.BendPara[5].Zone3Low = Var_RightY2_Low3.Value;
            recipe.BendPara[5].Zone3Up = Var_RightY2_Up3.Value;
            recipe.BendPara[5].Rate1 = Var_RightY2_Rate1.Value;
            recipe.BendPara[5].Rate2 = Var_RightY2_Rate2.Value;
            recipe.BendPara[5].Rate3 = Var_RightY2_Rate3.Value;
            recipe.BendPara[5].Adj_Num = Var_RightY2_Num.Value;

            recipe.BendPara[0].DirValue = Check_LeftY1_DIR.IsCkecked ? 1 : -1;
            recipe.BendPara[1].DirValue = Check_LeftY2_DIR.IsCkecked ? 1 : -1;
            recipe.BendPara[2].DirValue = Check_MidY1_DIR.IsCkecked ? 1 : -1;
            recipe.BendPara[3].DirValue = Check_MidY2_DIR.IsCkecked ? 1 : -1;
            recipe.BendPara[4].DirValue = Check_RightY1_DIR.IsCkecked ? 1 : -1;
            recipe.BendPara[5].DirValue = Check_RightY2_DIR.IsCkecked ? 1 : -1;

            recipe.BendPara[0].AOIOffset = var_LeftY1_AOI.Value;
            recipe.BendPara[1].AOIOffset = var_LeftY2_AOI.Value;
            recipe.BendPara[2].AOIOffset = var_MidY1_AOI.Value;
            recipe.BendPara[3].AOIOffset = var_MidY2_AOI.Value;
            recipe.BendPara[4].AOIOffset = var_RightY1_AOI.Value;
            recipe.BendPara[5].AOIOffset = var_RightY2_AOI.Value;

            recipe.BendPara[0].AOIBase = var_LeftY1_AOI_Base.Value;
            recipe.BendPara[1].AOIBase = var_LeftY2_AOI_Base.Value;
            recipe.BendPara[2].AOIBase = var_MidY1_AOI_Base.Value;
            recipe.BendPara[3].AOIBase = var_MidY2_AOI_Base.Value;
            recipe.BendPara[4].AOIBase = var_RightY1_AOI_Base.Value;
            recipe.BendPara[5].AOIBase = var_RightY2_AOI_Base.Value;


            recipe.WorkModel = rbt_XY.Checked ? 0 : (rbt_YX.Checked ? 1 : 2);
            recipe.XY_Model = check_WorkModel.IsCkecked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cardAIOSet1.Save();
            cardBIOSet1.Save();
            cardCIOSet1.Save();
            cardDIOSet1.Save();
            cardModulA1.Save();
            cardModulB1.Save();
            cardModulC1.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            axisSetPanel1.Save();
            axisSetPanel2.Save();
            axisSetPanel3.Save();
            axisSetPanel4.Save();
        }

        private void btn_savefunction_Click(object sender, EventArgs e)
        {

            UpdateCHK();
            UpdateRecipeValue();
            MeasurementContext.Config.Save();
            MeasurementContext.Data.Save();
        }

        private void btn_setleftbendpress_Click(object sender, EventArgs e)
        {
            MeasurementContext.Worker.SetPressure(0, MeasurementContext.Config.LeftBendPressure * 1000);
        }

        private void btn_setmidbendpress_Click(object sender, EventArgs e)
        {
            MeasurementContext.Worker.SetPressure(1, MeasurementContext.Config.MidBendPressure * 1000);
        }

        private void btn_setrightbendpress_Click(object sender, EventArgs e)
        {
            MeasurementContext.Worker.SetPressure(2, MeasurementContext.Config.RightBendPressure * 1000);
        }

        private void rbt_CheckedChanged(object sender, EventArgs e)
        {

            if (rbt_XY.Checked)
            {
                Check_LeftY1_DIR.IsCkecked = false;
                Check_LeftY2_DIR.IsCkecked = false;
                Check_MidY1_DIR.IsCkecked = false;
                Check_MidY2_DIR.IsCkecked = false;
                Check_RightY1_DIR.IsCkecked = false;
                Check_RightY2_DIR.IsCkecked = false;
                check_WorkModel.IsCkecked = true;
                label23.Text = "左折弯1";
                label28.Text = "左折弯2";
                label27.Text = "中折弯1";
                label26.Text = "中折弯2";
                label25.Text = "右折弯1";
                label24.Text = "右折弯2";


                UpdateRecipeValue();
            }
            else if (rbt_YX.Checked)
            {

                //Check_LeftY1_DIR.IsCkecked = true;
                //Check_LeftY2_DIR.IsCkecked = true;
                //Check_MidY1_DIR.IsCkecked = true;
                //Check_MidY2_DIR.IsCkecked = true;
                //Check_RightY1_DIR.IsCkecked = true;
                //Check_RightY2_DIR.IsCkecked = true;
                check_WorkModel.IsCkecked = true;
                label23.Text = "左折弯1";
                label28.Text = "左折弯2";
                label27.Text = "中折弯1";
                label26.Text = "中折弯2";
                label25.Text = "右折弯1";
                label24.Text = "右折弯2";
                UpdateRecipeValue();
            }
            else
            {

                Check_LeftY1_DIR.IsCkecked = false;
                Check_LeftY2_DIR.IsCkecked = false;
                Check_MidY1_DIR.IsCkecked = false;
                Check_MidY2_DIR.IsCkecked = false;
                Check_RightY1_DIR.IsCkecked = false;
                Check_RightY2_DIR.IsCkecked = false;
                check_WorkModel.IsCkecked = false;
                label23.Text = "左折弯Y1";
                label28.Text = "左折弯Y2";
                label27.Text = "中折弯Y1";
                label26.Text = "中折弯Y2";
                label25.Text = "右折弯Y1";
                label24.Text = "右折弯Y2";
                UpdateRecipeValue();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MeasurementContext.Worker.IsStop = false;
            double ffff = 0;
            MeasurementContext.Worker.LoadCellWork(ref ffff, MeasurementContext.Worker.LoadCell1Net);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MeasurementContext.Worker.IsStop = false;
            double ffff = 0;
            MeasurementContext.Worker.LoadCellWork(ref ffff, MeasurementContext.Worker.LoadCell2Net);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MeasurementContext.Worker.IsStop = false;
            double ffff = 0;
            MeasurementContext.Worker.LoadCellWork(ref ffff, MeasurementContext.Worker.LoadCell3Net);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int res = 0;
            MeasurementContext.Worker.LoadCellResetVal(ref res, MeasurementContext.Worker.LoadCell1Net);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int res = 0;
            MeasurementContext.Worker.LoadCellResetVal(ref res, MeasurementContext.Worker.LoadCell2Net);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int res = 0;
            MeasurementContext.Worker.LoadCellResetVal(ref res, MeasurementContext.Worker.LoadCell3Net);
        }




        private void btn_printpathselect_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fs = new FolderBrowserDialog(); fs.ShowDialog();
            txb_printpath.Text = MeasurementContext.Config.PrintPath = fs.SelectedPath;
        }

        private void checkButton2_Load(object sender, EventArgs e)
        {

        }
    }
}
